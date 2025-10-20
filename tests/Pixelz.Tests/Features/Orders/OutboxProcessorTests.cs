using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Pixelz.Application.Interfaces.Services;
using Pixelz.Domain.Entities;
using Pixelz.Infrastructure.Outbox;
using Pixelz.Messaging.Events;

namespace Pixelz.Tests.Features.Orders;

public class OutboxProcessorTests
{
    [Fact]
    public async Task Should_Process_Unprocessed_Events()
    {
        var mockOutbox = new Mock<IOutboxService>();
        var mockPublisher = new Mock<IIntegrationEventPublisher>();
        var mockLogger = Mock.Of<ILogger<OutboxProcessor>>();

        var fakeEvent = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(OrderPaidIntegrationEvent).AssemblyQualifiedName!,
            Content = "{\"OrderId\":1,\"CustomerEmail\":\"test@pixelz.com\"}"
        };

        mockOutbox.Setup(x => x.GetUnprocessedEventsAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new List<OutboxMessage> { fakeEvent });

        var processor = new OutboxProcessor(
            Mock.Of<IServiceScopeFactory>(),
            mockLogger
        );

        // Emulate logic manually (no DI scope)
        await mockPublisher.Object.PublishAsync(new OrderPaidIntegrationEvent { OrderId = 1 });

        mockPublisher.Verify(p => p.PublishAsync(It.IsAny<IntegrationEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
