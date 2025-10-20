namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents a stored integration event used by the Transactional Outbox pattern.
/// </summary>
/// <remarks>
/// Each <see cref="OutboxMessage"/> entry corresponds to a serialized integration event 
/// that was generated as part of a database transaction. 
/// 
/// These messages are later processed by a background worker 
/// (e.g., <c>OutboxProcessor</c>) to ensure reliable event publishing, 
/// even in the presence of transient failures or system restarts.
/// </remarks>
public class OutboxMessage : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the outbox message.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the UTC timestamp when the event occurred in the system.
    /// </summary>
    public DateTime OccurredOnUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the .NET type name of the event (e.g., <c>OrderPaidIntegrationEvent</c>).
    /// </summary>
    public string Type { get; set; } = default!;

    /// <summary>
    /// Gets or sets the serialized JSON representation of the event payload.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// Gets or sets the UTC timestamp when this message was successfully processed and published.
    /// If null, the message is still pending or retrying.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the system has attempted to process this message.
    /// </summary>
    public int AttemptCount { get; set; }

    /// <summary>
    /// Gets or sets the last recorded error message (if any) from a failed processing attempt.
    /// </summary>
    public string? LastError { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the last error occurred.
    /// </summary>
    public DateTime? LastErrorAtUtc { get; set; }
}
