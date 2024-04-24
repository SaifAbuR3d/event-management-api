namespace EventManagement.Domain.Entities;

public class Like : Entity
{
    internal Like()
    { }

    public Like(Attendee attendee, Event @event)
    {
        Attendee = attendee;
        Event = @event;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public Attendee Attendee { get; set; } = default!;
    public int AttendeeId { get; set; }
    public Event Event { get; set; } = default!;
    public int EventId { get; set; }
}
