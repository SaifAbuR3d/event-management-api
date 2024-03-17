using EventManagement.Application.Events.CreateEvent.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<int>> CreateEvent([FromForm] CreateEventRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(environment.WebRootPath);
        var id = await mediator.Send(command, cancellationToken);
        return Ok(new { eventId = id });
    }

}
