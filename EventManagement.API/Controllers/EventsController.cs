using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Events.GetAllEvents;
using EventManagement.Application.Features.Events.GetEvent;
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
        var id = await mediator.Send(command, cancellationToken);
        return Ok(new { eventId = id });
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
}
