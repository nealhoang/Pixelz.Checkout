namespace Pixelz.Infrastructure.Messaging.Handlers;

/// <summary>
/// Handles email notifications for order-related integration events.
/// </summary>
/// <remarks>
/// This handler listens for two types of events:
/// <list type="bullet">
/// <item><description><see cref="OrderPaidIntegrationEvent"/> — sends a success confirmation email.</description></item>
/// <item><description><see cref="OrderFailedIntegrationEvent"/> — sends a failure notification email.</description></item>
/// </list>
/// 
/// The handler simulates email delivery by logging messages to the application console.
/// In a real implementation, this could be integrated with an external SMTP or email API (e.g., AWS SES, SendGrid).
/// </remarks>
public class EmailEventHandler :
    INotificationHandler<OrderPaidIntegrationEvent>,
    INotificationHandler<OrderFailedIntegrationEvent>
{
    private readonly ILogger<EmailEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailEventHandler"/> class.
    /// </summary>
    /// <param name="logger">The application logger used for diagnostics and simulation.</param>
    public EmailEventHandler(ILogger<EmailEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the <see cref="OrderPaidIntegrationEvent"/> event by simulating the sending of a payment confirmation email.
    /// </summary>
    public async Task Handle(OrderPaidIntegrationEvent notification, CancellationToken ct)
    {
        _logger.LogInformation($"Sending payment success email to {notification.CustomerEmail} for Order {notification.OrderId}");

        // Simulate async operation (e.g., external API call)
        await Task.Delay(100, ct);
    }

    /// <summary>
    /// Handles the <see cref="OrderFailedIntegrationEvent"/> event by simulating the sending of a failure notification email.
    /// </summary>
    public async Task Handle(OrderFailedIntegrationEvent notification, CancellationToken ct)
    {
        _logger.LogWarning($"Sending payment failure email to {notification.CustomerEmail} for Order {notification.OrderId}: {notification.Reason}");

        // Simulate async operation (e.g., external API call)
        await Task.Delay(100, ct);
    }
}
