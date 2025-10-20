namespace Pixelz.Messaging.Events;

/// <summary>
/// Represents the base class for all integration events published across services.
/// </summary>
/// <remarks>
/// Integration events are immutable data contracts used for cross-service communication.  
/// Each derived event describes a business action that has occurred (e.g., order paid, invoice created).  
/// 
/// The <see cref="IntegrationEvent"/> base type provides consistent metadata such as:
/// <list type="bullet">
/// <item><description><see cref="Id"/> — unique identifier of the event instance</description></item>
/// <item><description><see cref="OccurredOnUtc"/> — UTC timestamp when the event happened</description></item>
/// <item><description><see cref="EventType"/> — simple .NET type name used for logging or serialization</description></item>
/// </list>
/// 
/// These events are typically stored in an outbox table and later dispatched by a background worker.
/// </remarks>
public abstract class IntegrationEvent
{
    /// <summary>
    /// Gets the unique identifier of this integration event instance.
    /// Used to ensure idempotent processing across distributed systems.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the UTC timestamp when this event occurred within the originating service.
    /// </summary>
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the event type name (derived from the .NET type) — useful for diagnostics or serialization.
    /// </summary>
    public string EventType => GetType().Name;
}
