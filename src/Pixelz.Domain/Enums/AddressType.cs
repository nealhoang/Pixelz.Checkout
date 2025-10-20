namespace Pixelz.Domain.Enums;

/// <summary>
/// Represents the type or purpose of a customer's address.
/// </summary>
/// <remarks>
/// An address can be used either for product delivery (<see cref="Shipping"/>) 
/// or for invoicing and billing (<see cref="Billing"/>).
/// </remarks>
public enum AddressType : byte
{
    /// <summary>
    /// The address used for delivering shipped products or orders.
    /// </summary>
    Shipping = 0,

    /// <summary>
    /// The address used for billing and invoice information.
    /// </summary>
    Billing = 1
}
