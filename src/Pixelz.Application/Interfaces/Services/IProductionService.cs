namespace Pixelz.Application.Interfaces.Services;

/// <summary>
/// Defines contract for communicating with the Production system.
/// </summary>
/// <remarks>
/// In a real-world setup, this would send the order data to an external
/// production or workflow management system (e.g., via REST API, message queue).
/// </remarks>
public interface IProductionService
{
    /// <summary>
    /// Pushes an order to the production system for further processing.
    /// </summary>
    /// <param name="orderId">The ID of the order to push.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns><c>true</c> if successfully submitted; otherwise <c>false</c>.</returns>
    Task<bool> PushToProductionAsync(long orderId, CancellationToken ct = default);
}
