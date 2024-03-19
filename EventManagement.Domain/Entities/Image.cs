namespace EventManagement.Domain.Entities;

public class Image : Entity
{
    public string ImageUrl { get; set; } = default!;
    public bool IsThumbnail { get; set; } = false;
}
