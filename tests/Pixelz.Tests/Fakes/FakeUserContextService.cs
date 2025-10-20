using Pixelz.Application.Interfaces.Services;

namespace Pixelz.Tests.Fakes;

public class FakeUserContextService : IUserContextService
{
    public bool IsAuthenticated => true;

    public string? GetCurrentUserId() => Guid.Empty.ToString();

    public string? GetCurrentUserName()
    {
        throw new NotImplementedException();
    }
}
