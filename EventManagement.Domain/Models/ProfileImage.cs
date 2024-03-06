namespace EventManagement.Domain.Models;

public class ProfileImage : Entity
{
    public string ImageUrl { get; set; } = default!;
    public Profile Profile { get; set; } = default!;
    public int ProfileId { get; set; }
}
