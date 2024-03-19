namespace EventManagement.Domain.Entities;

public class Organizer : Entity
{
    public string DisplayName { get; set; } = default!; // display name
    public bool IsVerified { get; set; } = default!;

    public int UserId { get; set; }
    public Profile? Profile { get; set; } = default!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Following> Followings { get; set; } = new List<Following>(); // Followers
}
