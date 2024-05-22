namespace EventManagement.Application.Contracts.Requests;

public class GetAllBookingsQueryParameters : GetAllQueryParameters
{
    public int? AttendeeId { get; set; }
}
