using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Responses;

public class EventDto
{
    public int Id { get; set; }
    public OrganizerDto Organizer { get; set; } = default!;
    public List<CategoryDto> Categories { get; set; } = []; 
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public bool HasStarted { get; set; }
    public bool HasEnded { get; set; }
    public bool IsRunning { get; set; }
    public bool TicketsSalesStarted { get; set; }
    public bool TicketsSalesEnded { get; set; }
    public bool TicketsSalesRunning { get; set; }

    public bool IsLikedByCurrentUser { get; set; }


    public bool IsOnline { get; set; }
    public double? Lat { get; set; }
    public double? Lon { get; set; }
    public string? Street { get; set; }
    public CityDto? City { get; set; }

    public string? ThumbnailUrl { get; set; }
    public List<string> ImageUrls { get; set; } = [];

    public List<TicketDto> Tickets { get; set; } = [];

    public bool IsManaged { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public Gender? AllowedGender { get; set; }
}
