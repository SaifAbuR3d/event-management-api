namespace EventManagement.Domain.Entities;

public class Following : Entity
{
    internal Following()
    { }

    public Following(Attendee attendee, Organizer organizer)
    {
        Attendee = attendee;
        Organizer = organizer;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public Attendee Attendee { get; set; } = default!;
    public int AttendeeId { get; set; }
    public Organizer Organizer { get; set; } = default!;
    public int OrganizerId { get; set; }
}
