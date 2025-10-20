namespace Pixelz.Infrastructure.Services;

public class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;

    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendPaymentSuccessEmailAsync(string toEmail, string orderNumber, decimal amount, CancellationToken ct = default)
    {
        _logger.LogInformation("[MockEmailService] Sending SUCCESS email to {Email} for Order {OrderNumber} (Amount: {Amount:C})",
            toEmail, orderNumber, amount);

        await Task.Delay(100, ct);
    }

    public async Task SendPaymentFailedEmailAsync(string toEmail, string orderNumber, string reason, CancellationToken ct = default)
    {
        _logger.LogWarning("[MockEmailService] Sending FAILURE email to {Email} for Order {OrderNumber}. Reason: {Reason}", toEmail, orderNumber, reason);

        await Task.Delay(100, ct);
    }
}
