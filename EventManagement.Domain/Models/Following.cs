namespace EventManagement.Domain.Models;

public class Following : Entity
{
    public Attendee Attendee { get; set; } = default!;
    public Guid AttendeeId { get; set; }
    public Organizer Organizer { get; set; } = default!;
    public Guid OrganizerId { get; set; }
}
