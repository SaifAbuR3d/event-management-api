namespace EventManagement.Application.Contracts.Responses;

public class LikeDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public AttendeeDto Attendee { get; set; } = default!;
    public EventDto Event { get; set; } = default!;
}
