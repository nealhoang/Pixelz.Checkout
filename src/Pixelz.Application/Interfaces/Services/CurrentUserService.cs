using Microsoft.AspNetCore.Http;

namespace Pixelz.Infrastructure.Services;

/// <summary>
/// Provides user context information using <see cref="IHttpContextAccessor"/>.
/// Falls back to a system user when running in background or without HTTP context.
/// </summary>
public class CurrentUserService : IUserContextService
{
    private static readonly string _systemUserId = Guid.Empty.ToString();

    public bool IsAuthenticated => true;

    public string GetCurrentUserId() => _systemUserId;

    public string? GetCurrentUserName() => "system";
}
