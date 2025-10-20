using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pixelz.Application.Features.Orders.Commands.CheckoutOrder;
using Pixelz.Application.Interfaces;
using Pixelz.Application.Interfaces.Repositories;
using Pixelz.Application.Interfaces.Services;
using Pixelz.Domain.Entities;
using Pixelz.Domain.Enums;
using Pixelz.Messaging.Events;
using Pixelz.Tests.Fakes;

namespace Pixelz.Tests.Features.Orders;

public class CheckoutOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IOutboxService> _outbox = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IUserContextService> _userContext = new();
    private readonly ILogger<CheckoutOrderHandler> _logger = Mock.Of<ILogger<CheckoutOrderHandler>>();

    [Fact]
    public async Task Handle_Should_Set_Order_To_Paid_When_Payment_Success()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            Status = OrderStatus.Created,
            Customer = new Customer { Email = "test@pixelz.com", FullName = "John Doe" }
        };

        _orderRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(order);

        _userContext.Setup(u => u.GetCurrentUserId()).Returns(Guid.Empty.ToString());

        var handler = new CheckoutOrderHandler(
            _logger,
            _orderRepo.Object,
            new FakePaymentService(true),
            _outbox.Object,
            _uow.Object,
            _userContext.Object
        );

        // Act
        var result = await handler.Handle(new CheckoutOrderCommand(1), default);

        // Assert
        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Paid);
        _outbox.Verify(o => o.AddEventAsync(It.IsAny<IntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Handle_Should_Set_Order_To_PaymentFailed_When_Payment_Fails()
    {
        // Arrange
        var order = new Order
        {
            Id = 2,
            Status = OrderStatus.Created,
            Customer = new Customer { Email = "fail@pixelz.com", FullName = "Jane Doe" }
        };

        _orderRepo.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(order);

        _userContext.Setup(u => u.GetCurrentUserId()).Returns(Guid.Empty.ToString());

        var handler = new CheckoutOrderHandler(
            _logger,
            _orderRepo.Object,
            new FakePaymentService(false),
            _outbox.Object,
            _uow.Object,
            _userContext.Object
        );

        // Act
        var result = await handler.Handle(new CheckoutOrderCommand(2), default);

        // Assert
        result.Should().BeFalse();
        order.Status.Should().Be(OrderStatus.PaymentFailed);
        _outbox.Verify(o => o.AddEventAsync(It.IsAny<IntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);

    }
}
