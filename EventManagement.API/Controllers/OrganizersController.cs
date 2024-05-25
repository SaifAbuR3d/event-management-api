using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Follow.GetOrganizerFollowers;
using EventManagement.Application.Features.Organizers.GetOrganizer;
using EventManagement.Application.Features.Organizers.UpdateProfile;
using EventManagement.Application.Features.SetUserProfilePicture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// Endpoints to retrieve/manage organizer information
/// </summary>
/// <param name="mediator"></param>
/// <param name="environment"></param>
[ApiController]
[Route("api/[controller]")]
public class OrganizersController(IMediator mediator,
    IWebHostEnvironment environment) : ControllerBase
{
    /// <summary>
    /// Gets an organizer by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:Int}")]
    public async Task<ActionResult<OrganizerDto>> GetOrganizerById(int id, CancellationToken cancellationToken)
    {
        var organizer = await mediator.Send(new GetOrganizerByIdQuery(id),
            cancellationToken);
        return Ok(organizer);
    }

    /// <summary>
    /// Gets an organizer by its username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public async Task<ActionResult<OrganizerDto>> GetOrganizerByUserName(string username, CancellationToken cancellationToken)
    {
        var organizer = await mediator.Send(new GetOrganizerByUserNameQuery(username),
            cancellationToken);
        return Ok(organizer);
    }

    /// <summary>
    /// Gets the followers of an organizer
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}/followers")]
    public async Task<ActionResult<IEnumerable<AttendeeDto>>> GetOrganizerFollowers(int id, 
        [FromQuery] GetAllQueryParameters parameters, CancellationToken cancellationToken)
    {
        var (attendees, paginationMetadata) = await mediator.Send(new GetAttendeesFollowingAnOrganizerQuery(id,
            parameters), cancellationToken); 

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(attendees);
    }

    /// <summary>
    /// Sets the profile of the logged in organizer
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("my/profile")]
    public async Task<ActionResult> UpdateOrganizerProfile(SetOrganizerProfileCommand command)
    {
        await mediator.Send(command);
        return Ok(new { message = "Operation Successful" });
    }
    /// <summary>
    /// Sets the profile picture of the logged in organizer
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
