using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Requests;

public class GetAllIvrsQueryParameters : GetAllQueryParameters
{
    public int? UserId { get; set; }
    public bool OnlyAttendees { get; set; } = false;
    public bool OnlyOrganizers { get; set; } = false;
    public DocumentType? DocumentType { get; set; }
    public IdentityVerificationRequestStatus? Status { get; set; }
}
