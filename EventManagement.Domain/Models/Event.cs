namespace EventManagement.Domain.Models;

public class Event : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


    // TODO: Change to Location object, with lat and long, and address, city, country, etc.
    public string Location { get; set; } = default!;   

    // TODO: Add EventLimitation object if needed, (e.g. 18+, 21+, female only, etc.)


    public Guid OrganizerId { get; set; }
    public Organizer Organizer { get; set; } = default!;
    public Guid EventCategoryId { get; set; }
    public EventCategory EventCategory { get; set; } = default!;
    public ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<RegistrationRequest>? RegistrationRequests { get; set; } = new List<RegistrationRequest>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

}
