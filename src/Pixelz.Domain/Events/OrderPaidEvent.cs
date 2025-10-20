namespace Pixelz.Domain.Events;

/// <summary>
/// Represents a domain event that is raised when an order has been successfully paid.
/// </summary>
/// <remarks>
/// This event is typically raised within the domain layer 
/// after a payment transaction succeeds, signaling that 
/// the order can now transition to the next stage of processing 
/// (e.g., invoicing or production submission).
/// </remarks>
public class OrderPaidEvent
{
    /// <summary>
    /// Gets the unique identifier of the order that was paid.
    /// </summary>
    public long OrderId { get; }

    /// <summary>
    /// Gets the UTC timestamp when the order was successfully paid.
    /// </summary>
    public DateTime PaidAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderPaidEvent"/> class.
    /// </summary>
    /// <param name="orderId">The unique identifier of the paid order.</param>
    /// <param name="paidAt">The UTC timestamp when payment was completed.</param>
    public OrderPaidEvent(long orderId, DateTime paidAt)
    {
        OrderId = orderId;
        PaidAt = paidAt;
    }
}
