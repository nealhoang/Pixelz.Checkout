namespace Pixelz.Application.Features.Orders.Commands.CheckoutOrder;

/// <summary>
/// Handles the checkout (payment) process for an order.
/// This class coordinates between domain validation, payment processing, and persistence.
/// </summary>
public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, bool>
{
    private readonly ILogger<CheckoutOrderHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IOutboxService _outboxService;
    private readonly IUnitOfWork _unitOfWork;    

    private readonly IUserContextService _userContextService;

    public CheckoutOrderHandler
    (
        ILogger<CheckoutOrderHandler> logger,
        IOrderRepository orderRepository,
        IPaymentService paymentService,
        IOutboxService outboxService,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService
    )
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _outboxService = outboxService;
        _unitOfWork = unitOfWork;

        _userContextService = userContextService;
    }

    public async Task<bool> Handle(CheckoutOrderCommand request, CancellationToken ct)
    {
        var order = await GetOrderOrThrowAsync(request.OrderId, ct);

        if (!CanCheckout(order))
        {
            _logger.LogWarning("Order {OrderId} is in status {Status}, checkout not allowed.", order.Id, order.Status);
            throw new InvalidOperationException($"Order {order.Id} is not in a valid state for checkout.");
        }

        string _currentUserId = _userContextService.GetCurrentUserId() ?? Guid.Empty.ToString(); ;

        await MarkOrderAsPendingPaymentAsync(order, _currentUserId, ct);

        var paymentResult = await ProcessPaymentAsync(order, ct);

        if (paymentResult.Status == Common.Models.PaymentStatus.Failed)
        {
            await HandleFailedPaymentAsync(order, _currentUserId, ct);
            return false;
        }

        await HandleSuccessfulPaymentAsync(order, _currentUserId, ct);

        return true;
    }

    #region Private Methods


    /// <summary>
    /// Marks the order as PendingPayment before calling the payment provider.
    /// </summary>
    private async Task MarkOrderAsPendingPaymentAsync(Order order, string userId, CancellationToken ct)
    {
        order.MarkAsPendingPayment(userId);
        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Order {OrderId} marked as PendingPayment.", order.Id);
    }

    /// <summary>
    /// Retrieves the order or throws if not found.
    /// </summary>
    private async Task<Order> GetOrderOrThrowAsync(long orderId, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, ct);
        if (order is null)
        {
            throw new NotFoundException($"Order {orderId} not found.");
        }            

        return order;
    }

    /// <summary>
    /// Ensures the order is in a valid status for checkout.
    /// </summary>
    private bool CanCheckout(Order order)
    {
        return order.Status is OrderStatus.Created
          or OrderStatus.PaymentFailed
          or OrderStatus.PendingPayment;
    }

    /// <summary>
    /// Calls payment service and logs result.
    /// </summary>
    private async Task<PaymentResult> ProcessPaymentAsync(Order order, CancellationToken ct)
    {
        _logger.LogInformation("Processing payment for Order {OrderId}...", order.Id);
        var result = await _paymentService.ProcessPaymentAsync(order, ct);
        _logger.LogInformation("Payment for Order {OrderId} completed with status: {Status}", order.Id, result.Status);
        return result;
    }

    /// <summary>
    /// Handles payment failure — updates order + records failed event.
    /// </summary>
    private async Task HandleFailedPaymentAsync(Order order, string userId, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);

        order.MarkAsPaymentFailed(userId);
        
        await _orderRepository.UpdateAsync(order, ct);

        var evt = new OrderFailedIntegrationEvent
        {
            OrderId = order.Id,
            CustomerEmail = order.Customer.Email,
            Reason = "Payment declined"
        };

        await _outboxService.AddEventAsync(evt, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        await _unitOfWork.CommitTransactionAsync(ct);

        _logger.LogWarning("Order {OrderId} payment failed. Event stored in outbox.", order.Id);
    }

    /// <summary>
    /// Handles successful payment — updates order + records success event.
    /// </summary>
    private async Task HandleSuccessfulPaymentAsync(Order order, string userId, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);

        order.MarkAsPaid(userId);       

        await _orderRepository.UpdateAsync(order, ct);

        var evt = new OrderPaidIntegrationEvent
        {
            OrderId = order.Id,
            CustomerEmail = order.Customer.Email,
            TotalAmount = order.TotalAmount
        };

        await _outboxService.AddEventAsync(evt, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        await _unitOfWork.CommitTransactionAsync(ct);

        _logger.LogInformation("Order {OrderId} marked as paid. Event stored in outbox.", order.Id);
    }

    #endregion
}
