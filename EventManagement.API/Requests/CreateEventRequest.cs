using EventManagement.Application.Events.CreateEvent;

namespace EventManagement.API.Requests;

/// <summary>
/// The request to create a new event.
/// </summary>
public class CreateEventRequest
{
    /// <summary>
    /// The name of the event.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The description of the event.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// The category ID of the event.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// The start date of the event.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date of the event.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// The start time of the event.
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of the event.
    /// </summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// The latitude of the event location.
    /// </summary>
    public double? Lat { get; set; }

    /// <summary>
    /// The longitude of the event location.
    /// </summary>
    public double? Lon { get; set; }

    /// <summary>
    /// The street address of the event location.
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// The city ID of the event location.
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    /// Indicates whether the event is online.
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// The thumbnail of the event.
    /// </summary>
    public IFormFile Thumbnail { get; set; } = default!;

    /// <summary>
    /// The images of the event.
    /// </summary>
    public List<IFormFile>? Images { get; set; }

    /// <summary>
    /// Converts the <see cref="CreateEventRequest"/> object to a <see cref="CreateEventCommand"/> object.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <returns>The created <see cref="CreateEventCommand"/> object.</returns>
    public CreateEventCommand ToCommand(string baseUrl)
    {
        return new CreateEventCommand(Name, Description, CategoryId,
            StartDate, EndDate, StartTime, EndTime,
            Lat, Lon, Street, CityId, IsOnline,
            Thumbnail, Images, baseUrl);
    }
}

