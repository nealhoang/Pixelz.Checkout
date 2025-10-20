namespace Pixelz.Application.Interfaces.Services;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default);
}
