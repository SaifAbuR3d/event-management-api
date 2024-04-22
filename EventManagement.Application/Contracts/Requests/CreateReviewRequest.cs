namespace EventManagement.Application.Contracts.Requests;

public class CreateReviewRequest
{
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = default!;

}
