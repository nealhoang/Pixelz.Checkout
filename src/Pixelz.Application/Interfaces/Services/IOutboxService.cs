namespace Pixelz.Application.Interfaces.Services;

/// <summary>
/// Defines the contract for the Outbox service that stores integration events
/// before being published by the Outbox Processor.
/// </summary>
public interface IOutboxService
{
    /// <summary>
    /// Adds a new integration event to the Outbox table.
    /// Should be called within the same transaction as the business operation.
    /// </summary>
    Task AddEventAsync(IntegrationEvent @event, CancellationToken ct = default);

    /// <summary>
    /// Retrieves all unprocessed outbox messages.
    /// Typically used by the background Outbox Processor.
    /// </summary>
    Task<List<OutboxMessage>> GetUnprocessedEventsAsync(CancellationToken ct = default);

    /// <summary>
    /// Marks the specified outbox message as processed.
    /// </summary>
    Task MarkAsProcessedAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Increments the retry counter for a failed message.
    /// Marks it as dead-lettered if it exceeds the maximum retry attempts.
    /// </summary>
    Task IncrementRetryCountAsync(Guid messageId, int maxRetry, CancellationToken ct = default);

    /// <summary>
    /// Records error details for a failed publishing attempt.
    /// </summary>
    Task RecordErrorAsync(Guid messageId, Exception ex, CancellationToken ct = default);
}
