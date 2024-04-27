namespace EventManagement.Domain.Entities;

public enum DocumentType
{
    BirthCertificate,
    Passport,
    NationalId,
    DriverLicense,
    SchoolCard,
    Other
}
public class Document : Entity
{
    internal Document()
    { }
    public Document(DocumentType documentType, string documentFileUrl)
    {
        DocumentType = documentType;
        DocumentFileUrl = documentFileUrl;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }
    public DocumentType DocumentType { get; private set; } = default!;
    public string DocumentFileUrl { get; private set; } = default!;
}
