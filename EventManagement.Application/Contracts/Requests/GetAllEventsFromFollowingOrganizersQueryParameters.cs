namespace EventManagement.Application.Contracts.Requests;
public class GetAllEventsFromFollowingOrganizersQueryParameters
    : GetAllQueryParameters
{
    public bool OnlyFutureEvents { get; set; } = true;
    public new string? SortColumn { get; set; } = "startDate";

}