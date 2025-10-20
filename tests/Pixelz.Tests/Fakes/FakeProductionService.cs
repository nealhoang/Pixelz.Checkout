using Pixelz.Application.Interfaces.Services;

namespace Pixelz.Tests.Fakes;

public class FakeProductionService : IProductionService
{
    public Task<bool> PushToProductionAsync(long orderId, CancellationToken ct = default)
    {
        return Task.FromResult(true);
    }
}
