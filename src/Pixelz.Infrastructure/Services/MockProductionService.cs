namespace Pixelz.Infrastructure.Services;

/// <summary>
/// Mock implementation of <see cref="IProductionService"/>.
/// Simulates sending the order to a production system.
/// </summary>
public class MockProductionService : IProductionService
{
    private readonly ILogger<MockProductionService> _logger;
    private static readonly Random _random = new();

    public MockProductionService(ILogger<MockProductionService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> PushToProductionAsync(long orderId, CancellationToken ct = default)
    {
        _logger.LogInformation("Sending Order {OrderId} to production...", orderId);

        // Simulate external API call latency
        await Task.Delay(_random.Next(300, 1000), ct);

        // Simulate random outcome (90% success)
        bool success = _random.NextDouble() < 0.9;

        if (success)
        {
            _logger.LogInformation("Order {OrderId} successfully submitted to production system.", orderId);
        }
        else
        {
            _logger.LogWarning("Failed to push Order {OrderId} to production (mock failure).", orderId);
        }

        return success;
    }
}
