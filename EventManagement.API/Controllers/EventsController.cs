using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Events.EventStatistics;
using EventManagement.Application.Features.Events.GetAllEvents;
using EventManagement.Application.Features.Events.GetEvent;
using EventManagement.Application.Features.Events.GetEventsFromFollowingOrganizers;
using EventManagement.Application.Features.Events.GetMostRatedEventsInTheLastNDays;
using EventManagement.Application.Features.Events.GetNearEvents;
using EventManagement.Application.Features.Events.GetOtherEventsMayLike;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// The controller for the events endpoints
/// </summary>
[ApiController]
[Route("api/events")]
public class EventsController(IMediator mediator,
    IWebHostEnvironment environment) : ControllerBase
{
    /// <summary>
    /// Creates a new event
    /// </summary>
    /// <param name="request">The data for the new event</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the creation</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateEvent([FromForm] CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(environment.WebRootPath);
        var eventId = await mediator.Send(command, cancellationToken);
        return Ok(new { eventId });
    }

    /// <summary>
    /// Gets the event with the specified id
    /// </summary>
    /// <param name="eventId">The id of the event</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The event with the specified id</returns>
    [HttpGet("{eventId}")]
    public async Task<ActionResult<EventDto>> GetEvent(int eventId, CancellationToken cancellationToken)
    {
        var query = new GetEventQuery(eventId);
        var @event = await mediator.Send(query, cancellationToken);
        return Ok(@event);
    }

    /// <summary>
    /// Gets all events, filtered, sorted, and paginated according to the specified parameters
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of events</returns>
    [HttpGet]
    public async Task<ActionResult<(IEnumerable<EventDto>, PaginationMetadata)>> GetAllEvents(
               [FromQuery] GetAllEventsQueryParameters parameters, CancellationToken cancellationToken)
    {
        var (events, paginationMetadata) = await mediator.Send(new GetAllEventsQuery(parameters),
            cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(events);
    }

    /// <summary>
    /// gets the statistics of the event with the specified id
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/stats")]
    public async Task<ActionResult<EventStatisticsDto>> GetEventStatistics(int eventId,
        CancellationToken cancellationToken)
    {
        var query = new GetEventStatisticsQuery(eventId);
        var stats = await mediator.Send(query, cancellationToken);
        return Ok(stats);
    }

    /// <summary>
    /// returns other events that the user may like, based on the event categories
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/may-like")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetOtherEventsMayLike(int eventId,
               CancellationToken cancellationToken)
    {
        var events = await mediator.Send(new GetOtherEventsMayLikeQuery(eventId), cancellationToken);
        return Ok(events);
    }
    /// <summary>
    /// returns other events that the user may like, based on the user interests
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("i-may-like")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetOtherEventsMayLikeByAttendee(
                      CancellationToken cancellationToken)
    {
        var events = await mediator.Send(new GetOtherEventsMayLikeByAttendeeQuery(), cancellationToken);
        return Ok(events);
    }

    /// <summary>
    /// Gets the events near the specified location
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("near")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetNearEvents(
               [FromQuery] GetNearEventsQueryParameters parameters,
               CancellationToken cancellationToken)
    {
        var query = new GetNearEventsQuery(parameters.Latitude, parameters.Longitude,
                       parameters.MaximumDistanceInKM, parameters.NumberOfEvents);
        var events = await mediator.Send(query, cancellationToken);
        return Ok(events);
    }

    /// <summary>
    /// Gets the most rated events in the last N days, with the specified number of events
    /// </summary>
    /// <param name="days"></param>
    /// <param name="numberOfEvents"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("most-rated")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetMostRatedEvents(CancellationToken cancellationToken, [FromQuery] int days = 7, [FromQuery] int numberOfEvents = 20)
    {
        var events = await mediator.Send(new GetMostRatedEventsInLastNDaysQuery(days, numberOfEvents),
            cancellationToken);
        return Ok(events);
    }

    /// <summary>
    /// Get the average rating of an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/avg")]
    public async Task<ActionResult<double>> GetEventAverageRating(int eventId,
        CancellationToken cancellationToken)
    {
        var query = new GetEventAverageRatingQuery(eventId);
        var average = await mediator.Send(query, cancellationToken);
        return Ok(new { average });
    }
    /// <summary>
    /// Gets the events from the following organizers,
    /// filtered, sorted, and paginated according to the specified parameters.
    /// To be used in the home page 
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("home-feedback")]
    public async Task<ActionResult<(IEnumerable<EventDto>, PaginationMetadata)>> GetHomeFeedback(
        [FromQuery] GetAllEventsFromFollowingOrganizersQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var (events, paginationMetadata) = await mediator.Send(
            new GetEventsFromFollowingOrganizersQuery(parameters), cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(events);
    }
}
