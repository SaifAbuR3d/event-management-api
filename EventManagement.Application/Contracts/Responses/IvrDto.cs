using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;

public class IvrDto
{
    public int Id { get; set; }

    // This is AttendeeId OR OrganizerId depending on the user type, NOT UserId :( 
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string? ProfilePictureUrl { get; set; } = default!;

    public string DocumentUrl { get; set; } = default!;
    public DocumentType DocumentType { get; set; } = default!;

    public IdentityVerificationRequestStatus Status { get; set; } = default!;
    public string? AdminMessage { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
