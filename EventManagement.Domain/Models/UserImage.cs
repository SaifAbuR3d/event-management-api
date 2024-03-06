namespace EventManagement.Domain.Models;

public class UserImage: Entity
{
    public string ImageUrl { get; set; } = default!;
    public int UserId { get; set; }
}
