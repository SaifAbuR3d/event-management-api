namespace EventManagement.Domain.Models;

public class Following : Entity
{
    public Attendee Attendee { get; set; } = default!;
    public int AttendeeId { get; set; }
    public Organizer Organizer { get; set; } = default!;
    public int OrganizerId { get; set; }
}
