using EventManagement.Application.Events.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

/// <summary>
/// The controller for the events endpoints
/// </summary>
[ApiController]
[Route("api/events")]
public class EventsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new event
    /// </summary>
    /// <param name="command">The data for the new event</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the creation</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateEvent(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(new { eventId = result });
    }
}
