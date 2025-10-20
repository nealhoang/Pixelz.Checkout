namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents a payment transaction associated with a specific order.
/// </summary>
/// <remarks>
/// Each <see cref="PaymentTransaction"/> records information about 
/// a single payment attempt, including the provider used, 
/// transaction details, status, and amount.
/// 
/// The entity also includes domain methods for safely transitioning 
/// between different payment states such as <c>Success</c>, <c>Failed</c>, 
/// or <c>Refunded</c>.
/// </remarks>
public class PaymentTransaction : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the payment transaction.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated order.
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the related <see cref="Order"/> entity.
    /// </summary>
    public Order Order { get; set; } = default!;

    /// <summary>
    /// Gets or sets the payment provider used for this transaction.
    /// </summary>
    public PaymentProvider Provider { get; set; } = PaymentProvider.MockPay;

    /// <summary>
    /// Gets or sets the unique transaction ID returned by the payment provider.
    /// </summary>
    public string TransactionId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the total amount processed in this payment transaction.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the current status of the payment transaction.
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Gets the UTC timestamp when the transaction was created.
    /// </summary>
    public DateTime CreatedAtUtc => CreatedAt;

    // ----------------------------
    // Domain Methods
    // ----------------------------

    /// <summary>
    /// Marks the payment as successfully completed.
    /// </summary>
    /// <param name="updatedBy">The identifier of the user or system performing the update.</param>
    public void MarkSuccess(string updatedBy)
    {
        Status = PaymentStatus.Success;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the payment as failed.
    /// </summary>
    /// <param name="updatedBy">The identifier of the user or system performing the update.</param>
    public void MarkFailed(string updatedBy)
    {
        Status = PaymentStatus.Failed;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the payment as refunded.
    /// </summary>
    /// <param name="updatedBy">The identifier of the user or system performing the update.</param>
    public void Refund(string updatedBy)
    {
        Status = PaymentStatus.Refunded;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
}
