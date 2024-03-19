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
    public DocumentType DocumentType { get; set; } = default!;
    public string DocumentFileUrl { get; set; } = default!;
}
