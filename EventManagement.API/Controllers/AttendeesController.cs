using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Follow.FollowAnOrganizer;
using EventManagement.Application.Features.Follow.GetFollowings;
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
    /// Gets the organizers followed by an attendee
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}/follows")]
    public async Task<ActionResult<IEnumerable<OrganizerDto>>> GetFollowings(int id, 
        [FromQuery] GetAllQueryParameters parameters, CancellationToken cancellationToken)
    {
        var (followings, paginationMetadata) = await mediator.Send(new GetOrganizersFollowedByAnAttendee(id, parameters), 
            cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(followings);
    }
}
