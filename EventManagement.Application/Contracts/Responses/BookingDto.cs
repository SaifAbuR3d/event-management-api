namespace EventManagement.Application.Contracts.Responses;

public class BookingDto
{
    public int Id { get; set; }
    public int AttendeeId { get; set; }
    public int EventId { get; set; }
    public ICollection<PersonTicketDto> Tickets { get; set; } = new List<PersonTicketDto>();
}
