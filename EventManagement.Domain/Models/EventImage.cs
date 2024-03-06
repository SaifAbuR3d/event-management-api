namespace EventManagement.Domain.Models;

public class EventImage : Entity
{
    public string ImageUrl { get; set; } = default!;
    public Event Event { get; set; } = default!;
    public Guid EventId { get; set; }
}
