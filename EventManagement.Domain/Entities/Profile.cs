namespace EventManagement.Domain.Entities;

public class Profile : Entity
{
    public string? Bio { get; set; } = default!;
    public string? ColorScheme { get; set; } = default!;
    public string? Website { get; set; }
    public string? Twitter { get; set; }
    public string? Facebook { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }

    public int OrganizerId { get; set; }
    public Organizer Organizer { get; set; } = default!;
    public ICollection<ProfileImage> ProfileImages { get; set; } = new List<ProfileImage>();
}
