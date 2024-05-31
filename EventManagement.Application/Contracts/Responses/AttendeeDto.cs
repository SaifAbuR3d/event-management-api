using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;

public class AttendeeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; } = default!;
    public bool IsVerified { get; set; }
    public string? ImageUrl { get; set; }
    public string? Email { get; set; } // returned only in Admin Dashboard
}
