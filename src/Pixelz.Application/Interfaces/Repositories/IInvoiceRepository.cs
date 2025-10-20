namespace Pixelz.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing invoice persistence operations.
/// </summary>
public interface IInvoiceRepository
{
    /// <summary>
    /// Adds a new invoice to the persistence context.
    /// </summary>
    /// <param name="invoice">The invoice entity to add.</param>
    /// <param name="ct">Cancellation token for cooperative cancellation.</param>
    Task AddAsync(Invoice invoice, CancellationToken ct = default);

}
