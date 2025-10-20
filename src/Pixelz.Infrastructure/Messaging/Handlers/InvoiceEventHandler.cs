namespace Pixelz.Infrastructure.Messaging.Handlers;

/// <summary>
/// Handles invoice creation when an order payment is confirmed.
/// </summary>
/// <remarks>
/// This handler listens for <see cref="OrderPaidIntegrationEvent"/>, triggered when
/// an order has been successfully paid. It simulates the creation of an invoice record
/// and (optionally) publishes a follow-up <see cref="InvoiceCreatedIntegrationEvent"/>.
/// 
/// In a production environment, this could integrate with an accounting system
/// or external billing API.
/// </remarks>
public class InvoiceEventHandler : INotificationHandler<OrderPaidIntegrationEvent>
{
    private readonly ILogger<InvoiceEventHandler> _logger;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxService _outboxService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceEventHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger for diagnostics and tracing.</param>
    /// <param name="invoiceRepository">Repository for persisting invoices.</param>
    /// <param name="unitOfWork">Unit of work for atomic database operations.</param>
    /// <param name="outboxService">Outbox service for deferred event publishing.</param>
    public InvoiceEventHandler(
        ILogger<InvoiceEventHandler> logger,
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        IOutboxService outboxService)
    {
        _logger = logger;
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
    }

    /// <summary>
    /// Handles the <see cref="OrderPaidIntegrationEvent"/> by creating an invoice record
    /// and optionally publishing a follow-up <see cref="InvoiceCreatedIntegrationEvent"/>.
    /// </summary>
    /// <param name="notification">The payment event containing order details.</param>
    /// <param name="ct">Cancellation token for cooperative cancellation.</param>
    public async Task Handle(OrderPaidIntegrationEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Received OrderPaidIntegrationEvent — creating invoice for Order {OrderId} (Total: ${Amount})", notification.OrderId, notification.TotalAmount);

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            // Create a new invoice entity
            var invoice = new Invoice
            {
                OrderId = notification.OrderId,
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{notification.OrderId}",
                TotalAmount = notification.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.Empty.ToString()
            };

            await _invoiceRepository.AddAsync(invoice, ct);

            // Optionally publish follow-up event to outbox
            var invoiceCreatedEvent = new InvoiceCreatedIntegrationEvent
            {
                InvoiceId = invoice.Id,
                OrderId = notification.OrderId,
                CustomerEmail = notification.CustomerEmail
            };

            await _outboxService.AddEventAsync(invoiceCreatedEvent, ct);

            await _unitOfWork.SaveChangesAsync(ct);
            await _unitOfWork.CommitTransactionAsync(ct);

            _logger.LogInformation($"Invoice {invoice.InvoiceNumber} created for Order {notification.OrderId}.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            _logger.LogError(ex, $"Failed to create invoice for Order {notification.OrderId}");
        }
    }
}
