namespace EventManagement.Application.Contracts.Responses;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
