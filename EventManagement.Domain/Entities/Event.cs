namespace EventManagement.Domain.Entities;

public enum EventStatus
{
    Draft,
    Published,
    Cancelled
}


public class Event : Entity
{
    internal Event() { }
    public Event(Organizer organizer, string name, string description,
        DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime,
        bool isOnline = false)
    {
        Organizer = organizer;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;
        IsOnline = isOnline;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public bool HasStarted()
    {
        var eventStart = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day,
                       StartTime.Hour, StartTime.Minute, StartTime.Second);

        return eventStart < DateTime.UtcNow;
    }
    public bool HasEnded()
    {
        var eventEnd = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day,
                                  EndTime.Hour, EndTime.Minute, EndTime.Second);

        return eventEnd < DateTime.UtcNow;
    }

    public bool IsRunning()
    {
        return HasStarted() && !HasEnded();
    }

    public bool TicketsSalesStarted()
    {
        return Tickets.Any(t => t.StartSale < DateTime.UtcNow);
    }

    public bool TicketsSalesEnded()
    {
        return Tickets.Any(t => t.EndSale < DateTime.UtcNow);
    }

    public bool TicketsSalesRunning()
    {
        return TicketsSalesStarted() && !TicketsSalesEnded();
    }

    public void SetLocation(double latitude, double longitude, string? street, int? cityId)
    {
        if (IsOnline)
            throw new InvalidOperationException("Event is online, location cannot be set.");

        Latitude = latitude;
        Longitude = longitude;
        Street = street;
        CityId = cityId;

        LastModified = DateTime.UtcNow;

    }

    public void SetLimitations(int? minAge, int? maxAge, Gender? allowedGender)
    {
        IsManaged = true;

        MinAge = minAge;
        MaxAge = maxAge;
        AllowedGender = allowedGender;

        LastModified = DateTime.UtcNow;
    }
    public void AddCategory(Category category)
    {
        Categories.Add(category);

        LastModified = DateTime.UtcNow;
    }

    public void SetThumbnail(string thumbnailUrl)
    {
        EventImages.Add(new EventImage(this, thumbnailUrl, true));

        LastModified = DateTime.UtcNow;
    }

    public void AddImage(string imageUrl)
    {
        EventImages.Add(new EventImage(this, imageUrl));

        LastModified = DateTime.UtcNow;
    }

    public void AddTicket(Ticket ticket)
    {
        Tickets.Add(ticket);

        LastModified = DateTime.UtcNow;
    }

    public void AddTicket(string name, decimal price, int quantity, DateTime startSale, DateTime endSale)
    {
        var ticket = new Ticket(this, name, price, quantity, startSale, endSale);
        Tickets.Add(ticket);

        LastModified = DateTime.UtcNow;
    }


    // Basic info
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public bool IsManaged { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public Gender? AllowedGender { get; set; }


    // Location info
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Street { get; set; }
    public bool IsOnline { get; set; }

    public EventStatus Status { get; set; }


    // Optional foreign key
    public int? CityId { get; set; }
    public City? City { get; set; } = default!;


    public int OrganizerId { get; set; }
    public Organizer Organizer { get; set; } = default!;
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<RegistrationRequest>? RegistrationRequests { get; set; } = new List<RegistrationRequest>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
