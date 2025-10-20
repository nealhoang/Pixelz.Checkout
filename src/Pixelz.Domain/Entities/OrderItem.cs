namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents an individual item within an order,
/// such as a single image or product to be processed or retouched.
/// </summary>
/// <remarks>
/// This entity is part of the <see cref="Order"/> aggregate and cannot exist independently.
/// Each <see cref="OrderItem"/> contains pricing and quantity information,
/// along with a description of the retouching service or processing type requested.
/// </remarks>
public class OrderItem : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the order item.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent <see cref="Order"/>.
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Gets or sets the navigation reference to the parent order.
    /// </summary>
    public Order Order { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the image or file associated with this order item.
    /// </summary>
    public string ImageFileName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of retouching or processing requested for this item
    /// (e.g., "Background Removal", "Color Correction").
    /// </summary>
    public string RetouchType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unit price for this item (per image or service).
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the quantity of items included in this line.
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Gets the total cost for this order item (calculated as UnitPrice × Quantity).
    /// </summary>
    public decimal SubTotal => UnitPrice * Quantity;
}
