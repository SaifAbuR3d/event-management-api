namespace EventManagement.Domain.Models;

public class Attendee : Entity
{
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = default!;
    public int UserId { get; set; }
    public ICollection<Following> Followings { get; set; } = new List<Following>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<RegistrationRequest> RegistrationRequests { get; set; } = new List<RegistrationRequest>();
}
