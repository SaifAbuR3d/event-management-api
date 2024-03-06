namespace EventManagement.Domain.Models;

public class EventCategory : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
