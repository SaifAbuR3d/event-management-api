using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;

public class RegRequestDto
{
    public int Id { get; set; }
    public int AttendeeId { get; set; }
    public string AttendeeUserName { get; set; } = default!;
    public string? AttendeeProfilePictureUrl { get; set; } = default!;
    public int EventId { get; set; }
    public RegistrationStatus Status { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
