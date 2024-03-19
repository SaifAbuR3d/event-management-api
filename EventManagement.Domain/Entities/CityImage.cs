namespace EventManagement.Domain.Entities;

public class CityImage : Image
{
    public City City { get; set; } = default!;
    public int CityId { get; set; }
}
