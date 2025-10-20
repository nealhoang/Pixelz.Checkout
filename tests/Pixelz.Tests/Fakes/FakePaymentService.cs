using Pixelz.Application.Interfaces.Services;
using Pixelz.Application.Common.Models;
using Pixelz.Domain.Entities;
using Pixelz.Domain.Enums;

namespace Pixelz.Tests.Fakes;

/// <summary>
/// A fake implementation of <see cref="IPaymentService"/> used in tests.
/// It simulates payment success or failure deterministically.
/// </summary>
public class FakePaymentService : IPaymentService
{
    private readonly bool _shouldSucceed;
    private readonly PaymentProvider _provider;

    /// <summary>
    /// Initializes a new fake payment service.
    /// </summary>
    /// <param name="shouldSucceed">Determines whether payments succeed or fail.</param>
    /// <param name="provider">Optional fake payment provider (defaults to MockPay).</param>
    public FakePaymentService(bool shouldSucceed = true, PaymentProvider provider = PaymentProvider.MockPay)
    {
        _shouldSucceed = shouldSucceed;
        _provider = provider;
    }

    /// <summary>
    /// Simulates a payment process for an order.
    /// </summary>
    public Task<PaymentResult> ProcessPaymentAsync(Order order, CancellationToken ct = default)
    {
        // Simulate transaction id
        var transactionId = $"FAKE-TX-{order.Id:D6}";

        if (_shouldSucceed)
        {
            // Successful payment
            return Task.FromResult(new PaymentResult(
                Provider: _provider,
                TransactionId: transactionId,
                Status: Application.Common.Models.PaymentStatus.Success
            ));
        }
        else
        {
            // Failed payment
            return Task.FromResult(new PaymentResult(
                Provider: _provider,
                TransactionId: transactionId,
                Status: Application.Common.Models.PaymentStatus.Failed,
                FailureReason: "Mock payment declined"
            ));
        }
    }
}
