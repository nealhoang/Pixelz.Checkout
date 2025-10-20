namespace Pixelz.Infrastructure.Outbox;

/// <summary>
/// Background service that processes unhandled Outbox messages periodically.
/// Responsible for reading pending messages, deserializing them into IntegrationEvents,
/// and publishing them through the registered event publisher (e.g., MediatR).
/// </summary>
public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
    private const int MaxRetryCount = 3;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OutboxProcessor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBatchAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error in OutboxProcessor loop.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    /// <summary>
    /// Reads unprocessed messages and processes them one by one.
    /// </summary>
    private async Task ProcessBatchAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
        var publisher = scope.ServiceProvider.GetRequiredService<IIntegrationEventPublisher>();

        var messages = await outboxService.GetUnprocessedEventsAsync(ct);

        if (messages.Count == 0)
        {
            _logger.LogDebug("No pending Outbox messages found.");
            return;
        }

        _logger.LogInformation("Found {Count} outbox messages to process.", messages.Count);

        foreach (var message in messages)
        {
            await ProcessMessageAsync(message, outboxService, publisher, ct);
        }
    }

    /// <summary>
    /// Processes a single outbox message: resolves event type, deserializes, publishes, marks processed.
    /// </summary>
    private async Task ProcessMessageAsync(
        OutboxMessage message,
        IOutboxService outboxService,
        IIntegrationEventPublisher publisher,
        CancellationToken ct)
    {
        try
        {
            var eventType = ResolveEventType(message.Type);
            if (eventType == null)
            {
                _logger.LogWarning("Could not resolve event type '{Type}' for message {Id}.", message.Type, message.Id);
                await outboxService.RecordErrorAsync(message.Id, new Exception("Unresolved event type"), ct);
                return;
            }

            var @event = DeserializeEvent(message.Content, eventType);
            if (@event == null)
            {
                _logger.LogWarning("Deserialization failed for message {Id}.", message.Id);
                await outboxService.RecordErrorAsync(message.Id, new Exception("Failed to deserialize event"), ct);
                return;
            }

            await PublishAndMarkProcessedAsync(@event, message, publisher, outboxService, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process OutboxMessage {MessageId}.", message.Id);
            await outboxService.RecordErrorAsync(message.Id, ex, ct);
            await outboxService.IncrementRetryCountAsync(message.Id, MaxRetryCount, ct);
        }
    }

    /// <summary>
    /// Attempts to resolve the event's CLR type from its name.
    /// </summary>
    private Type? ResolveEventType(string typeName)
    {
        // Handle both short type names ("OrderPaidIntegrationEvent")
        // and full type names ("Pixelz.Messaging.Events.OrderPaidIntegrationEvent, Pixelz.Messaging").
        var cleanName = typeName.Split(',')[0].Trim();

        var eventType = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(a => a.GetType(cleanName, throwOnError: false))
            .FirstOrDefault(t => t != null);

        if (eventType == null)
        {
            _logger.LogDebug("Could not resolve type for name {Name}.", cleanName);
        }            

        return eventType;
    }

    /// <summary>
    /// Safely deserializes the JSON payload into an IntegrationEvent.
    /// </summary>
    private IntegrationEvent? DeserializeEvent(string json, Type eventType)
    {
        try
        {
            return (IntegrationEvent?)JsonSerializer.Deserialize(
                json,
                eventType,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize JSON for type {Type}.", eventType.Name);
            return null;
        }
    }

    /// <summary>
    /// Publishes the IntegrationEvent via MediatR (or other bus),
    /// and marks the message as processed if successful.
    /// </summary>
    private async Task PublishAndMarkProcessedAsync(
        IntegrationEvent @event,
        OutboxMessage message,
        IIntegrationEventPublisher publisher,
        IOutboxService outboxService,
        CancellationToken ct)
    {
        _logger.LogInformation("Publishing event {EventType} (MessageId: {MessageId})", @event.GetType().Name, message.Id);

        await publisher.PublishAsync(@event, ct);
        await outboxService.MarkAsProcessedAsync(message.Id, ct);

        _logger.LogInformation("Event {EventType} processed successfully.", @event.GetType().Name);
    }
}
