using Pixelz.Application.Interfaces.Services;

namespace Pixelz.Infrastructure.EventHandlers;

/// <summary>
/// Handles <see cref="OrderPaidIntegrationEvent"/> events.
/// When an order is successfully paid, this handler updates
/// the order status to SubmittedToProduction.
/// </summary>
public class ProductionEventHandler : INotificationHandler<OrderPaidIntegrationEvent>
{
    private readonly ILogger<ProductionEventHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;
    private readonly IProductionService _productionService;

    public ProductionEventHandler(
        ILogger<ProductionEventHandler> logger,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IProductionService productionService,
        IUserContextService userContext)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;        
        _productionService = productionService;
        _userContext = userContext;
    }

    public async Task Handle(OrderPaidIntegrationEvent notification, CancellationToken ct)
    {
        _logger.LogInformation($"Received OrderPaidIntegrationEvent for Order {notification.OrderId}");

        var order = await _orderRepository.GetByIdAsync(notification.OrderId, ct);
        if (order == null)
        {
            _logger.LogWarning($"Order {notification.OrderId} not found. Skipping Production submission.");
            return;
        }

        if (order.Status == OrderStatus.Paid)
        {
            string _currentUserId = _userContext.GetCurrentUserId() ?? Guid.Empty.ToString();

            order.MarkAsSubmittedToProduction(_currentUserId);
            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation($"Order {order.Id} marked as SubmittedToProduction.");

            try
            {
                bool pushed = await _productionService.PushToProductionAsync(order.Id, ct);

                if (pushed)
                {
                    _logger.LogInformation($"Order {order.Id} successfully submitted to production system.");
                }
                else
                {
                    _logger.LogWarning($"Order {order.Id} failed to reach production. Will retry later.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while pushing Order { order.Id} to production.");
            }
        }
    }
}
