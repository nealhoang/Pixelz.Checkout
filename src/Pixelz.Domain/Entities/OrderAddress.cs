namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents an immutable address record associated with a specific order.
/// </summary>
/// <remarks>
/// Unlike <see cref="CustomerAddress"/>, which represents a customer's saved address book entry, 
/// <see cref="OrderAddress"/> is a snapshot of the shipping or billing address 
/// at the time the order was placed.
/// 
/// This ensures that historical order data remains consistent even if 
/// the customer later updates their profile or address.
/// </remarks>
public class OrderAddress
{
    /// <summary>
    /// Gets or sets the unique identifier for this order address record.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated <see cref="Order"/>.
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the related <see cref="Order"/>.
    /// </summary>
    public Order Order { get; set; } = default!;

    /// <summary>
    /// Gets or sets the address type — either <c>Shipping</c> or <c>Billing</c>.
    /// </summary>
    public AddressType Type { get; set; }

    /// <summary>
    /// Gets or sets the recipient’s full name for this address.
    /// </summary>
    public string FullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the contact phone number for this address.
    /// </summary>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Gets or sets the first address line (street, building, etc.).
    /// </summary>
    public string Line1 { get; set; } = default!;

    /// <summary>
    /// Gets or sets the optional second address line (apartment, suite, etc.).
    /// </summary>
    public string? Line2 { get; set; }

    /// <summary>
    /// Gets or sets the city of the address.
    /// </summary>
    public string City { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state, province, or region of the address.
    /// </summary>
    public string State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the country of the address.
    /// </summary>
    public string Country { get; set; } = default!;

    /// <summary>
    /// Gets or sets the postal or ZIP code.
    /// </summary>
    public string PostalCode { get; set; } = default!;
}
