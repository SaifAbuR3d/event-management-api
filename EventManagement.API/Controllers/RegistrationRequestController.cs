using EventManagement.Application.Features.RegistrationRequests.CreateRegistrationRequest;
using EventManagement.Application.Features.RegistrationRequests.SetRegRequestStatus;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for registration requests (attendees request to attend an event)
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/reg-requests")]
public class RegistrationRequestController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Creates a registration request
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> AddRegistrationRequestAsync(
        CreateRegistrationRequestCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }

    /// <summary>
    /// approve a registration request
    /// </summary>
    /// <param name="regRequestId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{regRequestId}/approve")]
    public async Task<ActionResult> Approve(int regRequestId, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new SetRegRequestCommand(regRequestId, RegistrationStatus.Approved),
            cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }

    /// <summary>
    /// reject a registration request
    /// </summary>
    /// <param name="regRequestId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{regRequestId}/reject")]
    public async Task<ActionResult> Reject(int regRequestId, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new SetRegRequestCommand(regRequestId, RegistrationStatus.Rejected),
            cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }
}
