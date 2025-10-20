namespace Pixelz.Application.Interfaces.Services;

public interface IPaymentService
{
      Task<PaymentResult> ProcessPaymentAsync(Order order, CancellationToken ct = default);
}
