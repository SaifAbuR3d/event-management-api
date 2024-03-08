namespace EventManagement.Domain.Models;

public enum IdentityVerificationRequestStatus
{
    Pending,
    Approved,
    Rejected
}

public class IdentityVerificationRequest : Entity
{
    public IdentityVerificationRequestStatus Status { get; set; } = default!;
    public string? UserMessage { get; set; }
    public string? AdminMessage { get; set; }
    public int UserId { get; set; }
    public int DocumentId { get; set; }
    public Document Document { get; set; } = default!;
}
