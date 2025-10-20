namespace Pixelz.Domain.Entities;

/// <summary>
/// Represents a customer within the Pixelz ecosystem.
/// </summary>
/// <remarks>
/// The <see cref="Customer"/> aggregate root encapsulates identity, contact information, 
/// and relationships to orders and saved addresses. 
/// 
/// This entity forms the foundation of the order and billing processes,
/// and its lifecycle typically begins with account creation in the Pixelz Portal.
/// </remarks>
public class Customer : AuditableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the customer's email address, which serves as the primary identifier for login and communication.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the full name of the customer.
    /// </summary>
    public string FullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of addresses associated with this customer.
    /// </summary>
    public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();

    /// <summary>
    /// Gets or sets the collection of orders placed by this customer.
    /// </summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    #region Domain Methods

    /// <summary>
    /// Updates the customer's full name and records the audit metadata.
    /// </summary>
    /// <param name="fullName">The new full name of the customer.</param>
    /// <param name="updatedBy">The user or system identifier performing the update.</param>
    public void UpdateName(string fullName, string updatedBy)
    {
        FullName = fullName;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    #endregion
}
