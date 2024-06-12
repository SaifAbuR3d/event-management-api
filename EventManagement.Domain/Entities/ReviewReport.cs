namespace EventManagement.Domain.Entities;

public class ReviewReport : Report
{
    internal ReviewReport() { }
    public ReviewReport(string content, int reviewId, int attendeeId)
        : base(content)
    {
        Content = content;
        ReviewId = reviewId;
        AttendeeId = attendeeId;
    }
    public int ReviewId { get; set; }
    public Review Review { get; set; } = default!;
}
