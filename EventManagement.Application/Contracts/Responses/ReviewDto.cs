namespace EventManagement.Application.Contracts.Responses;

public class ReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public int EventId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
    public int AttendeeId { get; set; }
    public string AttendeeName { get; set; } = default!;
    public string AttendeeUserName { get; set; } = default!;
    public string? AttendeeImageUrl { get; set; } = default!;
}
