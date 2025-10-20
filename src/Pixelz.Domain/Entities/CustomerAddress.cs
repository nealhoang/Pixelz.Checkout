namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents an address associated with a customer (billing or shipping).
/// </summary>
public class CustomerAddress : AuditableEntity
{
    public long Id { get; set; }

    /// <summary>
    /// Associated customer identifier.
    /// </summary>
    public long CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Line1 { get; set; } = default!;

    public string? Line2 { get; set; }

    public string City { get; set; } = default!;

    public string State { get; set; } = default!;

    public string Country { get; set; } = default!;

    public string PostalCode { get; set; } = default!;

    public AddressType Type { get; set; } = AddressType.Shipping;

    /// <summary>
    /// Indicates whether this address is set as default.
    /// </summary>
    public bool IsDefault { get; set; } = false;
}
