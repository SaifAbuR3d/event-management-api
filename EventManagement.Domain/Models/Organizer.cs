namespace EventManagement.Domain.Models;

public class Organizer : Entity
{
    public string? CompanyName { get; set; } = default!;
    public Guid UserId { get; set; }
    public Profile? Profile { get; set; } = default!;
    public Guid? ProfileId { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Following> Followings { get; set; } = new List<Following>(); // Followers
}
