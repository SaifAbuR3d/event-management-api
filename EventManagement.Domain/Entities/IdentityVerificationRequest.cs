namespace EventManagement.Domain.Entities;

public enum IdentityVerificationRequestStatus
{
    Pending,
    Approved,
    Rejected
}

public class IdentityVerificationRequest : Entity
{
    internal IdentityVerificationRequest()
    { }

    public IdentityVerificationRequest(int userId, string documentFileUrl, DocumentType documentType)
    {
        UserId = userId;

        Status = IdentityVerificationRequestStatus.Pending;

        SetDocument(documentFileUrl, documentType); 

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    private void SetDocument(string documentFileUrl, DocumentType documentType)
    {
        Document = new Document(documentType, documentFileUrl);
    }

    public void Approve(string? adminMessage)
    {
        if(Status != IdentityVerificationRequestStatus.Pending)
        {
            throw new InvalidOperationException("Cannot approve a request that is not pending");
        }

        Status = IdentityVerificationRequestStatus.Approved;
        AdminMessage = adminMessage;

        LastModified = DateTime.UtcNow;
    }

    public void Reject(string? adminMessage)
    {
        if(Status != IdentityVerificationRequestStatus.Pending)
        {
            throw new InvalidOperationException("Cannot reject a request that is not pending");
        }

        Status = IdentityVerificationRequestStatus.Rejected;
        AdminMessage = adminMessage;

        LastModified = DateTime.UtcNow;
    }


    public bool IsForOrganizer { get; set;}
    public IdentityVerificationRequestStatus Status { get; private set; } = default!;
    public string? AdminMessage { get; private set; }
    public int UserId { get; private set; }
    public int DocumentId { get; private set; }
    public Document Document { get; private set; } = default!;
}
