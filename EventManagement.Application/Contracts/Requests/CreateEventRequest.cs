using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Events.CreateEvent;
using EventManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EventManagement.Application.Contracts.Requests;

/// <summary>
/// The request to create a new event.
/// </summary>
public class CreateEventRequest
{

    /// <summary>
    /// Converts the <see cref="CreateEventRequest"/> object to a <see cref="CreateEventCommand"/> object.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <returns>The created <see cref="CreateEventCommand"/> object.</returns>
    public CreateEventCommand ToCommand(string baseUrl)
    {
        List<CreateTicketRequest> tickets = []; 

        try
        {
            tickets = JsonSerializer.Deserialize<List<CreateTicketRequest>>(Tickets, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? [];
        } catch (JsonException e)
        {
            throw new BadRequestException($"Invalid tickets JSON format {e.Message}");
        }


        return new CreateEventCommand(Name, Description, CategoryId,
            StartDate, EndDate, StartTime, EndTime,
            Lat, Lon, Street, CityId, IsOnline,
            Thumbnail, Images,
            tickets,
            IsManaged, MinAge, MaxAge, AllowedGender,
            baseUrl);
    }


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
    /// Indicates whether the event is managed (require registration request to attend).
    /// </summary>
    public bool IsManaged { get; set; }

    /// <summary>
    /// The minimum age to attend the event.
    /// </summary>
    public int? MinAge { get; set; }

    /// <summary>
    /// The maximum age to attend the event.
    /// </summary>
    public int? MaxAge { get; set; }

    /// <summary>
    /// The allowed gender to attend the event (both genders are allowed if null).
    public Gender? AllowedGender { get; set; }


    /// <summary>
    /// The thumbnail of the event.
    /// </summary>
    public IFormFile Thumbnail { get; set; } = default!;

    /// <summary>
    /// The images of the event.
    /// </summary>
    public List<IFormFile>? Images { get; set; }

    /// <summary>
    /// The tickets of the event. (As JSON string, array of objects with name, quantity, price, startSale, endSale)
    /// e.x. [{"name":"VIP","quantity":100,"price":100,"startSale":"2024-05-05T00:00:00","endSale":"2024-06-06T00:00:00"}]
    /// </summary>
    public string Tickets { get; set; } = default!;
}
