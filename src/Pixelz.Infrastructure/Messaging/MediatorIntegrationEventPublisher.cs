namespace Pixelz.Infrastructure.Messaging;

/// <summary>
/// An internal event publisher that leverages MediatR to publish <see cref="IntegrationEvent"/>s.
/// Used primarily for in-process (same application) event dispatching.
/// </summary>
/// <remarks>
/// This publisher wraps the IntegrationEvent inside an <see cref="IntegrationNotification"/> 
/// and sends it through MediatR's publish pipeline.
/// 
/// The actual handling logic for each event is implemented in corresponding 
/// <c>INotificationHandler&lt;IntegrationNotification&gt;</c> classes, such as:
/// <list type="bullet">
/// <item><description><c>EmailEventHandler</c></description></item>
/// <item><description><c>InvoiceEventHandler</c></description></item>
/// <item><description><c>ProductionEventHandler</c></description></item>
/// </list>
/// </remarks>
public class MediatorIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IMediator _mediator;
    private readonly ILogger<MediatorIntegrationEventPublisher> _logger;

    public MediatorIntegrationEventPublisher(IMediator mediator, ILogger<MediatorIntegrationEventPublisher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Publishes the given <see cref="IntegrationEvent"/> through MediatR.
    /// </summary>
    /// <param name="event">The integration event to be published.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        _logger.LogInformation("Publishing integration event {EventType}", @event.GetType().Name);

        await _mediator.Publish(@event, ct);
    }   
}
