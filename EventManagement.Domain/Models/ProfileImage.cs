namespace EventManagement.Domain.Models;

public class ProfileImage : Image
{
    public Profile Profile { get; set; } = default!;
    public int ProfileId { get; set; }
}
