namespace EventManagement.Application.Contracts.Responses;

public class OrganizerDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public bool IsVerified { get; set; } = default!;
    public ProfileDto Profile { get; set; } = default!;
    public string? ImageUrl { get; set; }
}
