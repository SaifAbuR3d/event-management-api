namespace EventManagement.Application.Contracts.Requests;

public class GetAttendeeFollowingsQueryParameters : GetAllQueryParameters
{
    public int? OrganizerId { get; set; }
    public string? OrganizerUserName { get; set; }
}
