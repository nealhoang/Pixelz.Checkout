namespace Pixelz.Messaging.Events;

/// <summary>
/// Represents an integration event that is raised when a new invoice has been successfully created.
/// </summary>
/// <remarks>
/// This event is typically published by the <c>InvoiceService</c> or <c>OrderService</c>
/// after an order payment has been successfully completed.
/// 
/// Consumers (e.g., Email or Accounting services) may listen to this event
/// to trigger follow-up actions such as sending a confirmation email
/// or recording the invoice in a financial ledger.
/// </remarks>
public class InvoiceCreatedIntegrationEvent : IntegrationEvent, INotification
{
    /// <summary>
    /// Gets the unique identifier of the invoice that was created.
    /// </summary>
    public long InvoiceId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the order for which this invoice was issued.
    /// </summary>
    public long OrderId { get; init; }

    /// <summary>
    /// Gets the email address of the customer associated with the invoice.
    /// </summary>
    public string CustomerEmail { get; init; } = default!;
}
