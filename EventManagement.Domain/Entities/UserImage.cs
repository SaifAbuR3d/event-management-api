namespace EventManagement.Domain.Entities;

public class UserImage : Image
{
    internal UserImage()
    { }

    public UserImage(int userId, string imageUrl)
    {
        ImageUrl = imageUrl;
        UserId = userId;
        IsThumbnail = true;
        CreationDate = DateTime.Now;
        LastModified = DateTime.Now;
    }
    public int UserId { get; set; }
}
