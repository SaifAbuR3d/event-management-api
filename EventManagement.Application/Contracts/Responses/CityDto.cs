namespace EventManagement.Application.Contracts.Responses;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? State { get; set; }
    public string? ZipCode { get; set; }
}
