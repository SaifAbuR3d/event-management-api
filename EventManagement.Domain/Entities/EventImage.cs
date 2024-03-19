namespace EventManagement.Domain.Entities;

public class EventImage : Image
{
    internal EventImage()
    { }


    public EventImage(Event @event, string imageUrl, bool isThumbnail = false)
    {
        ImageUrl = imageUrl;
        Event = @event;
        IsThumbnail = isThumbnail;
        CreationDate = DateTime.Now;
        LastModified = DateTime.Now;
    }

    public Event Event { get; set; } = default!;
    public int EventId { get; set; }
}
