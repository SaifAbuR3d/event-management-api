namespace EventManagement.Domain.Models;

public class Organizer : Entity
{
    public string? CompanyName { get; set; } = default!;

    public int UserId { get; set; }
    public Profile? Profile { get; set; } = default!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Following> Followings { get; set; } = new List<Following>(); // Followers
}
