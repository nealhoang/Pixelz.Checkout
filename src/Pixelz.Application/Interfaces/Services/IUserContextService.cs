namespace Pixelz.Application.Interfaces.Services;

/// <summary>
/// Provides information about the currently authenticated user,
/// used for auditing, background jobs, and domain operations.
/// </summary>
public interface IUserContextService
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// For background jobs, this may return a system identifier.
    /// </summary>
    string GetCurrentUserId();

    /// <summary>
    /// Gets the display name or email of the current user, if available.
    /// </summary>
    string? GetCurrentUserName();

    /// <summary>
    /// Indicates whether the current context is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }
}
