using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Features.IVRs.SetIvrStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for identity verification requests
/// </summary>
[ApiController]
[Route("api/iv-requests")]
public class IdentityVerificationRequestsController(IMediator mediator,
    IWebHostEnvironment environment) : ControllerBase
{
    /// <summary>
    /// requests to verify the identity of a user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> RequestIV(CreateIvrRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(environment.WebRootPath);
        var ivrId = await mediator.Send(command, cancellationToken);
        return Ok(ivrId);
    }

    /// <summary>
    /// admin approves an identity verification request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="adminMessage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/approve")]
    public async Task<ActionResult> ApproveIvr(int id, 
        [FromBody] string? adminMessage, CancellationToken cancellationToken)
    {
        var command = new ApproveIvrCommand(id, adminMessage);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// admin approves an identity verification request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="adminMessage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/reject")]
    public async Task<ActionResult> RejectIvr(int id,
        [FromBody] string? adminMessage, CancellationToken cancellationToken)
    {
        var command = new RejectIvrCommand(id, adminMessage);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

}
