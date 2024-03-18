namespace EventManagement.Application.Contracts.Responses;

public class OrganizerDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = default!;
    public bool IsVerified { get; set; } = default!;
}
