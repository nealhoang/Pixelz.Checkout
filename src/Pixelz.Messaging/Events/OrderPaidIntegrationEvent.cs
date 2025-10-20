namespace Pixelz.Messaging.Events;

/// <summary>
/// Represents an integration event that is raised when an order payment is successfully completed.
/// </summary>
/// <remarks>
/// This event is published by the <c>OrderService</c> or <c>CheckoutOrderHandler</c>
/// once a payment transaction has been confirmed as successful.
/// 
/// External consumers (such as Email, Invoice, or Production services)
/// can subscribe to this event to perform subsequent actions like:
/// <list type="bullet">
/// <item><description>Issuing an invoice</description></item>
/// <item><description>Sending a confirmation email to the customer</description></item>
/// <item><description>Pushing the order into the production workflow</description></item>
/// </list>
/// </remarks>
public class OrderPaidIntegrationEvent : IntegrationEvent, INotification
{
    /// <summary>
    /// Gets the unique identifier of the order that was successfully paid.
    /// </summary>
    public long OrderId { get; init; }

    /// <summary>
    /// Gets the email address of the customer who made the payment.
    /// </summary>
    public string CustomerEmail { get; init; } = default!;

    /// <summary>
    /// Gets the total amount that was paid for the order.
    /// </summary>
    public decimal TotalAmount { get; init; }
}
