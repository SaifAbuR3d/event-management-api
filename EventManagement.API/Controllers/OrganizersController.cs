using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Organizers.GetOrganizer;
using EventManagement.Application.Features.Organizers.GetOrganizerFollowers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// Endpoints to retrieve/manage organizer information
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/[controller]")]
public class OrganizersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets an organizer by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:Int}")]
    public async Task<ActionResult<OrganizerDto>> GetOrganizerById(int id)
    {
        var organizer = await mediator.Send(new GetOrganizerByIdQuery(id));
        return Ok(organizer);
    }

    /// <summary>
    /// Gets an organizer by its username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    public async Task<ActionResult<OrganizerDto>> GetOrganizerByUserName(string username)
    {
        var organizer = await mediator.Send(new GetOrganizerByUserName(username));
        return Ok(organizer);
    }
    /// <summary>
    /// Gets the followers of an organizer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/followers")]
    public async Task<ActionResult<IEnumerable<AttendeeDto>>> GetOrganizerFollowers(int id)
    {
        var (followers, paginationMetadata) = await mediator.Send(new GetOrganizerFollowersQuery(id));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(followers);
    }
}
