using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Follow.FollowAnOrganizer;
using EventManagement.Application.Features.Follow.GetFollowings;
using EventManagement.Application.Features.Follow.UnfollowAnOrganizer;
using EventManagement.Application.Features.Like.LikeAnEvent;
using EventManagement.Application.Features.Like.RemoveLike;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// E
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AttendeesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// follow an organizer by its id, attendee must be logged in
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("my/follows")]
    public async Task<ActionResult> FollowOrganizer(FollowAnOrganizerCommand command)
    {
        await mediator.Send(command);
        return Ok(new { message = "Operation Successful" });
    }

    /// <summary>
    /// unfollow an organizer by its id, attendee must be logged in
    /// </summary>
    /// <param name="organizerId"></param>
    /// <returns></returns>
    [HttpDelete("my/follows/{organizerId}")]
    public async Task<ActionResult> UnfollowOrganizer(int organizerId)
    {
        await mediator.Send(new UnFollowAnOrganizerCommand(organizerId));
        return NoContent();
    }

    /// <summary>
    /// Gets the organizers followed by an attendee
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}/follows")]
    public async Task<ActionResult<IEnumerable<OrganizerDto>>> GetFollowings(int id, 
        [FromQuery] GetAttendeeFollowingsQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var (followings, paginationMetadata) = await mediator.Send(new GetOrganizersFollowedByAnAttendee(id, parameters), 
            cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(followings);
    }

    /// <summary>
    /// like an event by its id, attendee must be logged in
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("my/likes")]
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
    [HttpDelete("my/likes/{eventId}")]
    public async Task<ActionResult> UnlikeAnEvent(int eventId)
    {
        await mediator.Send(new RemoveLikeFromEventCommand(eventId));
        return NoContent();
    }
}
