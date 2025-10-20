namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents an invoice issued for a successfully paid order.
/// </summary>
/// <remarks>
/// Each <see cref="Invoice"/> is linked to exactly one <see cref="Order"/>, 
/// capturing the total billed amount at the time of successful payment.
/// 
/// This entity is immutable once issued, ensuring accounting and audit integrity.
/// </remarks>
public class Invoice : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the invoice.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the invoice number (e.g., <c>INV-10001</c>).
    /// </summary>
    public string InvoiceNumber { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the associated <see cref="Order"/>.
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the related order.
    /// </summary>
    public Order Order { get; set; } = default!;

    /// <summary>
    /// Gets or sets the total amount billed on the invoice.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the invoice was officially issued.
    /// </summary>
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
}
