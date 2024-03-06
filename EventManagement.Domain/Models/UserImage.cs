namespace EventManagement.Domain.Models;

public class UserImage: Entity
{
    public string ImageUrl { get; set; } = default!;
    public Guid UserId { get; set; }
}
