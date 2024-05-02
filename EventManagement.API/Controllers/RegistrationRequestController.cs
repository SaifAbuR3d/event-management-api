using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Features.RegistrationRequests.CreateRegistrationRequest;
using EventManagement.Application.Features.RegistrationRequests.GetAllRegRequests;
using EventManagement.Application.Features.RegistrationRequests.SetRegRequestStatus;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for registration requests (attendees request to attend an event)
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/events")]
public class RegistrationRequestController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Creates a registration request
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/reg-requests")]
    public async Task<ActionResult> AddRegistrationRequestAsync([FromRoute] int eventId, CancellationToken cancellationToken)
    {
        var command = new CreateRegistrationRequestCommand(eventId);
        await mediator.Send(command, cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }


    /// <summary>
    /// approve a registration request
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="regRequestId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{eventId}/reg-requests/{regRequestId}/approve")]
    public async Task<ActionResult> Approve(int eventId, int regRequestId, CancellationToken cancellationToken)
    {
        var command = new SetRegRequestCommand(eventId, regRequestId, RegistrationStatus.Approved);
        await mediator.Send(command, cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }

    /// <summary>
    /// reject a registration request
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="regRequestId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{eventId}/reg-requests/{regRequestId}/reject")]
    public async Task<ActionResult> Reject(int eventId, int regRequestId, CancellationToken cancellationToken)
    {
        var command = new SetRegRequestCommand(eventId, regRequestId, RegistrationStatus.Rejected);
        await mediator.Send(command, cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }


    /// <summary>
    /// get all registration requests for an event, filtered, sorted, and paginated according to the specified optional parameters
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{eventId}/reg-requests")]
    public async Task<ActionResult<IEnumerable<RegistrationRequest>>> GetAllRegRequests(
        int eventId, [FromQuery] GetAllRegRequestQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = new GetAllRegRequestsQuery(eventId, parameters);
        var (regRequests, paginationMetadata) = await mediator.Send(query, cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(regRequests);
    }
}
