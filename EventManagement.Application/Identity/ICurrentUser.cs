namespace EventManagement.Application.Identity;

public interface ICurrentUser
{
    int UserId { get; }
    string UserName { get; }
    string Email { get; }
    string Role { get; }
    bool IsAttendee { get; }
    bool IsOrganizer { get; }
    bool IsAdmin { get; }
}
