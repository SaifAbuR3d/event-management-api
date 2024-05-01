namespace EventManagement.Domain.Entities;

public enum RegistrationStatus
{
    Pending,
    Approved,
    Rejected
}
public class RegistrationRequest : Entity
{
    internal RegistrationRequest()
    { }

    public RegistrationRequest(Attendee attendee, Event @event)
    {
        Attendee = attendee;
        Event = @event;

        Status = RegistrationStatus.Pending;
        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public void Approve()
    {
        if (Status != RegistrationStatus.Pending)
        {
            throw new InvalidOperationException("Cannot approve a request that is not pending");
        }

        Status = RegistrationStatus.Approved;
        LastModified = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status != RegistrationStatus.Pending)
        {
            throw new InvalidOperationException("Cannot approve a request that is not pending");
        }

        Status = RegistrationStatus.Rejected;
        LastModified = DateTime.UtcNow;
    }

    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public RegistrationStatus Status { get; set; }
}
