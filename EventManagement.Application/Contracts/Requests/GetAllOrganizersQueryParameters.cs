namespace EventManagement.Application.Contracts.Requests;

public class GetAllOrganizersQueryParameters : GetAllQueryParameters
{
    public bool? OnlyVerified { get; set; } = null; 
}