
namespace EventManagement.Domain.Entities;

public class Organizer : Entity
{
    public string DisplayName { get; set; } = default!; // display name
    public bool IsVerified { get; set; } = default!;

    public int UserId { get; set; }
    public Profile? Profile { get; set; } = default!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Following> Followings { get; set; } = new List<Following>(); // Followers

    public void SetProfile(string? bio, string? website, string? linkedIn, string? instagram, string? facebook, string? twitter)
    {
        Profile ??= new Profile
        {
            Bio = bio,
            Website = website,
            LinkedIn = linkedIn,
            Instagram = instagram,
            Facebook = facebook,
            Twitter = twitter
        };

        if (bio != null)
            Profile.Bio = bio;
        if (website != null)
            Profile.Website = website;
        if (linkedIn != null)
            Profile.LinkedIn = linkedIn;
        if (instagram != null)
            Profile.Instagram = instagram;
        if (facebook != null)
            Profile.Facebook = facebook;
        if (twitter != null)
            Profile.Twitter = twitter;
    }
}
