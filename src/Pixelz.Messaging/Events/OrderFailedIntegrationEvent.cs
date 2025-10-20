namespace Pixelz.Messaging.Events;

/// <summary>
/// Represents an integration event that is raised when an order payment has failed.
/// </summary>
/// <remarks>
/// This event is typically published by the <c>OrderService</c> or <c>CheckoutHandler</c>
/// when a payment transaction is declined or encounters an unexpected error.
/// 
/// External systems (e.g., Email or Support services) may subscribe to this event
/// to notify the customer or trigger corrective workflows.
/// </remarks>
public class OrderFailedIntegrationEvent : IntegrationEvent, INotification
{
    /// <summary>
    /// Gets the unique identifier of the order that failed.
    /// </summary>
    public long OrderId { get; init; }

    /// <summary>
    /// Gets the email address of the customer associated with the failed order.
    /// </summary>
    public string CustomerEmail { get; init; } = default!;

    /// <summary>
    /// Gets the reason for the failure (e.g., "Payment declined", "Card expired", etc.).
    /// </summary>
    public string Reason { get; init; } = default!;
}
