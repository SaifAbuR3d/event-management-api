using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Enums;
using System.Security.Claims;

namespace EventManagement.API.Services;

/// <summary>
/// Get information about the current authenticated user
/// </summary>
/// <param name="httpContextAccessor"></param>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    /// <summary>
    /// Get the Id of the current authenticated user
    /// </summary>
    public int UserId => int.Parse(
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthenticatedException()
        );

    /// <summary>
    /// Get the email of the current authenticated user
    /// </summary>
    public string Email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
            ?? throw new UnauthenticatedException();

    /// <summary>
    /// Get the userName of the current authenticated user
    /// </summary>
    public string UserName => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new UnauthenticatedException();

    /// <summary>
    /// Get the role of the current authenticated user
    /// </summary>
    public string Role => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)
            ?? throw new UnauthenticatedException();

    /// <summary>
    /// returns true is the current authenticated user is a guest
    /// </summary>
    public bool IsAttendee => Role == UserRole.Attendee.ToString();

    /// <summary>
    /// returns true is the current authenticated user is an organizer
    /// </summary>
    public bool IsOrganizer => Role == UserRole.Organizer.ToString();

    /// <summary>
    /// returns true is the current authenticated user is an admin
    /// </summary>
    public bool IsAdmin => Role == UserRole.Admin.ToString();
}
