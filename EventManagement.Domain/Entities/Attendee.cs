using EventManagement.Domain.Enums;

namespace EventManagement.Domain.Entities;

public enum Gender
{
    Male,
    Female
}
public class Attendee : Entity
{
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; } = default!;
    public bool IsVerified { get; set; }
    public int UserId { get; set; }

    public ICollection<Following> Followings { get; set; } = new List<Following>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<RegistrationRequest> RegistrationRequests { get; set; } = new List<RegistrationRequest>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
