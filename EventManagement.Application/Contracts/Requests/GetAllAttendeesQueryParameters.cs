namespace EventManagement.Application.Contracts.Requests;

public class GetAllAttendeesQueryParameters : GetAllQueryParameters
{
    public int? EventId { get; set; }
    public bool OnlyVerified { get; set; } = false;
}
