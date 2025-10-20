namespace Pixelz.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendPaymentSuccessEmailAsync(string toEmail, string orderNumber, decimal amount, CancellationToken ct = default);

    Task SendPaymentFailedEmailAsync(string toEmail, string orderNumber, string reason, CancellationToken ct = default);
}
