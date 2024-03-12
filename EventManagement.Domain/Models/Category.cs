namespace EventManagement.Domain.Models;

public class Category : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
}
