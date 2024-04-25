namespace EventManagement.Application.Contracts.Requests;

public class GetAllEventsQueryParameters : GetAllQueryParameters
{
    public int? CategoryId { get; set; }
    public int? OrganizerId { get; set; }
    public int? LikedBy { get; set; }
    public int? EventId { get; set; }
    public bool PreviousEvents { get; set; }
    public bool UpcomingEvents { get; set; }
    public bool RunningEvents { get; set; }
}
