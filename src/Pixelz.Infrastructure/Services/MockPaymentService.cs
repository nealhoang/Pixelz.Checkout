namespace Pixelz.Infrastructure.Services;

/// <summary>
/// A mock implementation of the IPaymentService used for local development,
/// integration tests, and EF migrations (no real gateway required).
/// </summary>
public class MockPaymentService : IPaymentService
{
    private readonly ILogger<MockPaymentService> _logger;
    private static readonly Random _random = new();

    public MockPaymentService(ILogger<MockPaymentService> logger)
    {
        _logger = logger;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(Order order, CancellationToken ct = default)
    {
        // Simulate network delay (100–500ms)
        await Task.Delay(_random.Next(100, 500), ct);

        // 80% success rate
        bool isSuccess = _random.NextDouble() > 0.2;

        var provider = PaymentProvider.MockPay;
        var transactionId = Guid.NewGuid().ToString("N");

        if (isSuccess)
        {
            _logger.LogInformation(
                "MockPaymentService] Payment succeeded via {Provider} for Order {OrderId} | TxId={TransactionId} | Amount={Amount:C}",
                provider, order.Id, transactionId, order.TotalAmount);

            return new PaymentResult(
                Provider: provider,
                TransactionId: transactionId,
                Status: Application.Common.Models.PaymentStatus.Success
            );
        }

        string reason = "Payment declined by mock provider";
        _logger.LogWarning(
            "[MockPaymentService] Payment FAILED via {Provider} for Order {OrderId} | TxId={TransactionId} | Amount={Amount:C} | Reason={Reason}",
            provider, order.Id, transactionId, order.TotalAmount, reason);

        return new PaymentResult(
            Provider: provider,
            TransactionId: transactionId,
            Status: Application.Common.Models.PaymentStatus.Failed,
            FailureReason: reason
        );
    }
}
