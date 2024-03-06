namespace EventManagement.Domain.Models;

public enum IdentityVerificationRequestStatus
{
    Pending,
    Approved,
    Rejected
}

public class IdentityVerificationRequest : Entity
{
    public string Status { get; set; } = default!;
    public string? UserMessage { get; set; }
    public string? AdminMessage { get; set; }
    public Guid UserId { get; set; }
    public Guid DocumentId { get; set; }
    public Document Document { get; set; } = default!;
}
