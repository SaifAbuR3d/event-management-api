namespace EventManagement.Domain.Models;

public class Review : Entity
{
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public Guid AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
}