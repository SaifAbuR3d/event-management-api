using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Attendees.GetAttendee;
using EventManagement.Application.Features.Attendees.GetAttendees;
using EventManagement.Application.Features.Follow.FollowAnOrganizer;
using EventManagement.Application.Features.Follow.GetFollowings;
using EventManagement.Application.Features.Follow.UnfollowAnOrganizer;
using EventManagement.Application.Features.Organizers.GetOrganizer;
using EventManagement.Application.Features.SetUserProfilePicture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// E
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AttendeesController(IMediator mediator,
    IWebHostEnvironment environment) : ControllerBase
{
    /// <summary>
    /// get all attendees, paginated, sorted, and optionally filtered by event id and verified status
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttendeeDto>>> GetAttendees(
         [FromQuery] GetAllAttendeesQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = new GetAttendeesQuery(parameters);
        var (attendees, paginationMetadata) = await mediator.Send(query, cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(attendees);
    }

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
    /// Gets an attendee by its username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public async Task<ActionResult<AttendeeDto>> GetAttendeeByUserName(string username, CancellationToken cancellationToken)
    {
        var attendee = await mediator.Send(new GetAttendeeByUserNameQuery(username),
            cancellationToken);
        return Ok(attendee);
    }

    /// <summary>
    /// Sets the profile picture of the logged in attendee
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    [HttpPost("my/profile-picture")]
    public async Task<ActionResult> SetProfilePicture(IFormFile image)
    {
        var command = new SetProfilePictureCommand(image, environment.WebRootPath);
        var imageUrl = await mediator.Send(command);

        return Ok(new { imageUrl });
    }
}
