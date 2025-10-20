namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents a customer's order, including payment, items, and status transitions.
/// </summary>
public class Order : AuditableEntity
{
    public long Id { get; set; }

    /// <summary>
    /// Unique code for identifying the order.
    /// </summary>
    public string OrderNumber { get; set; } = default!;

    /// <summary>
    /// Display name or description of the order.
    /// </summary>
    public string OrderName { get; set; } = default!;

    /// <summary>
    ///  Foreign key to the customer placing the order.
    /// </summary>
    public long CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;

    /// <summary>
    /// Current order status (Created, Paid, Completed, etc.)
    /// .</summary>
    public OrderStatus Status { get; set; } = OrderStatus.Created;

    /// <summary>
    /// Total payable amount for this order.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Date when payment was successfully completed.
    /// </summary>
    public DateTime? PaidAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public ICollection<PaymentTransaction> Payments { get; set; } = new List<PaymentTransaction>();

    public ICollection<OrderAddress> Addresses { get; set; } = new List<OrderAddress>();

    public Invoice? Invoice { get; set; }

    #region Domain Methods

    private void Touch(string updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the order as pending payment. 
    /// </summary>
    /// <param name="updatedBy"></param>
    public void MarkAsPendingPayment(string updatedBy)
    {
        Status = OrderStatus.PendingPayment;
        Touch(updatedBy);
    }

    /// <summary>
    /// Marks the order as paid and updates audit info.
    /// </summary>
    public void MarkAsPaid(string updatedBy)
    {
        Status = OrderStatus.Paid;
        PaidAt = DateTime.UtcNow;
        Touch(updatedBy);
    }

    /// <summary>
    /// Marks the order as payment failed.
    /// </summary>
    public void MarkAsPaymentFailed(string updatedBy)
    {
        Status = OrderStatus.PaymentFailed;
        Touch(updatedBy);
    }

    /// <summary>
    /// Marks the order as submitted to production.
    /// </summary>
    /// <param name="updatedBy"></param>
    public void MarkAsSubmittedToProduction(string updatedBy)
    {
        Status = OrderStatus.SubmittedToProduction;
        Touch(updatedBy);
    }

    /// <summary>
    /// Marks the order as in production.   
    /// </summary>
    /// <param name="updatedBy"></param>
    public void MarkAsInProduction(string updatedBy)
    {
        Status = OrderStatus.InProduction;
        Touch(updatedBy);
    }

    /// <summary>
    /// Marks the order as completed.
    /// </summary>
    /// <param name="updatedBy"></param>
    public void MarkAsCompleted(string updatedBy)
    {
        Status = OrderStatus.Completed;
        Touch(updatedBy);
    }

    /// <summary>
    /// Marks the order as cancelled.
    /// </summary>
    /// <param name="updatedBy"></param>
    public void Cancel(string updatedBy)
    {
        Status = OrderStatus.Cancelled;
        Touch(updatedBy);
    }

    #endregion
}
