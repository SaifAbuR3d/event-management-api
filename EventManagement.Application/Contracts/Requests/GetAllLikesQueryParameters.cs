namespace EventManagement.Application.Contracts.Requests;

public class GetAllLikesQueryParameters : GetAllQueryParameters
{
    public int AttendeeId { get; set; }
    public int EventId { get; set; }
}
