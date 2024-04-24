using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Features.Reports.AddReport;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using EventManagement.Application.Features.Reports.GetReports;
using EventManagement.Application.Features.Like.LikeAnEvent;
using EventManagement.Application.Features.Like.RemoveLike;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for likes
/// </summary>
/// <param name="mediator"></param>
[Route("api")]
[ApiController]
public class LikesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// like an event by its id, attendee must be logged in
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("attendees/my/likes")]
    public async Task<ActionResult> LikeAnEvent(LikeAnEventCommand command)
    {
        await mediator.Send(command);
        return Ok(new { message = "Operation Successful" });
    }

    /// <summary>
    /// remove like from an event by its id, attendee must be logged in
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    [HttpDelete("attendees/my/likes/{eventId}")]
    public async Task<ActionResult> UnlikeAnEvent(int eventId)
    {
        await mediator.Send(new RemoveLikeFromEventCommand(eventId));
        return NoContent();
    }
}
