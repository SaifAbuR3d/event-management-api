namespace EventManagement.Domain.Models;

public class CityImage : Entity
{
    public string ImageUrl { get; set; } = default!;
    public City City { get; set; } = default!;
    public int CityId { get; set; }
}
