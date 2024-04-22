namespace EventManagement.Domain.Entities;

public class Review : Entity
{
    internal Review()
    { }

    public Review(int rating, string comment, int eventId, int attendeeId)
    {
        Rating = rating;
        Comment = comment;
        EventId = eventId;
        AttendeeId = attendeeId;

        CreationDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public Review(int rating, string title, string comment, int eventId, int attendeeId)
        : this(rating, comment, eventId, attendeeId)
    {
        Title = title;
    }

    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
}