namespace Pixelz.Domain.Enums;

/// <summary>
/// Represents the possible states of an order throughout its lifecycle.
/// </summary>
/// <remarks>
/// This enum defines the key milestones an order goes through, 
/// from creation to fulfillment or cancellation.
/// Typical state transitions include:
/// <c>Created → PendingPayment → Paid → SubmittedToProduction → InProduction → Completed</c>
/// or <c>Created → PendingPayment → PaymentFailed → Cancelled</c>.
/// </remarks>
public enum OrderStatus : byte
{
    /// <summary>
    /// The order has been created but not yet submitted for payment.
    /// </summary>
    Created = 0,

    /// <summary>
    /// The order is waiting for the customer to complete payment.
    /// </summary>
    PendingPayment = 1,

    /// <summary>
    /// The payment was successful and the order is ready to move to production.
    /// </summary>
    Paid = 2,

    /// <summary>
    /// The payment attempt failed or was declined by the provider.
    /// </summary>
    PaymentFailed = 3,

    /// <summary>
    /// The order has been submitted to the production workflow.
    /// </summary>
    SubmittedToProduction = 4,

    /// <summary>
    /// The order is currently being processed in production.
    /// </summary>
    InProduction = 5,

    /// <summary>
    /// The order has been successfully completed and delivered.
    /// </summary>
    Completed = 6,

    /// <summary>
    /// The order has been manually or automatically cancelled.
    /// </summary>
    Cancelled = 7
}
