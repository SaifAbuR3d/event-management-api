namespace EventManagement.Application.Contracts.Requests;

public class GetAllIvrsQueryParameters : GetAllQueryParameters
{
    public int? UserId { get; set; }
    public string? Status { get; set; }
}
