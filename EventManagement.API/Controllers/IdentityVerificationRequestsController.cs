using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.IVRs.GetIvrs;
using EventManagement.Application.Features.IVRs.SetIvrStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/approve")]
    public async Task<ActionResult> ApproveIvr(int id, 
        SetIvrStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new ApproveIvrCommand(id, request.AdminMessage);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// admin approves an identity verification request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/reject")]
    public async Task<ActionResult> RejectIvr(int id,
        SetIvrStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new RejectIvrCommand(id, request.AdminMessage);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
    /// <summary>
    /// get all identity verification requests, with optional filters, pagination and sorting
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IvrDto>>> GetIVRs(
        [FromQuery] GetAllIvrsQueryParameters request, CancellationToken cancellationToken)
    {
        var (ivrs, paginationMetadata) = await mediator.Send(new GetIvrsQuery(request), cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(ivrs);
    }

}
