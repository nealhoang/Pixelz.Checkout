using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pixelz.Application.Interfaces;
using Pixelz.Application.Interfaces.Repositories;
using Pixelz.Domain.Entities;
using Pixelz.Domain.Enums;
using Pixelz.Infrastructure.EventHandlers;
using Pixelz.Messaging.Events;
using Pixelz.Tests.Fakes;

namespace Pixelz.Tests.Features.Orders;

public class ProductionEventHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly ILogger<ProductionEventHandler> _logger = Mock.Of<ILogger<ProductionEventHandler>>();

    [Fact]
    public async Task Handle_Should_Update_Status_To_SubmittedToProduction()
    {
        // Arrange
        var order = new Order { Id = 42, Status = OrderStatus.Paid };
        _orderRepo.Setup(x => x.GetByIdAsync(42, It.IsAny<CancellationToken>())).ReturnsAsync(order);

        var handler = new ProductionEventHandler(
            _logger,
            _orderRepo.Object,
            _uow.Object,
            new FakeProductionService(),
            new FakeUserContextService()
        );

        // Act
        await handler.Handle(new OrderPaidIntegrationEvent { OrderId = 42, CustomerEmail = "test@pixelz.com" }, default);

        // Assert
        order.Status.Should().Be(OrderStatus.SubmittedToProduction);
        _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
