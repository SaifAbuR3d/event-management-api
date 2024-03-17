namespace EventManagement.Domain.Models;

public class CityImage : Image
{
    public City City { get; set; } = default!;
    public int CityId { get; set; }
}
