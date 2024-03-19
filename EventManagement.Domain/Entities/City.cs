namespace EventManagement.Domain.Entities;

public class City : Entity
{
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? State { get; set; }
    public string? ZipCode { get; set; }

    public ICollection<CityImage> CityImages { get; set; } = new List<CityImage>();
    public ICollection<Event> Events { get; set; } = new List<Event>();

}
