using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Features.Identity.Login;
using EventManagement.Application.Features.Identity.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

/// <summary>
/// The controller for the authentication endpoints
/// </summary>

[ApiController]
[Route("api/auth")]

public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Registers a new admin user
    /// </summary>
    /// <param name="command">The data for the new admin user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the registration</returns>
    [HttpPost("register-admin")]
    public async Task<ActionResult<RegisterAdminResponse>> RegisterAdmin(
        RegisterAdminCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Registers a new attendee user
    /// </summary>
    /// <param name="command">The data for the new attendee user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the registration</returns>
    [HttpPost("register-attendee")]
    public async Task<ActionResult<RegisterAttendeeResponse>> RegisterAttendee(
        RegisterAttendeeCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Registers a new organizer user
    /// </summary>
    /// <param name="command">The data for the new organizer user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the registration</returns>
    [HttpPost("register-organizer")]
    public async Task<ActionResult<RegisterOrganizerResponse>> RegisterOrganizer(
        RegisterOrganizerCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="command">The data for the login</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the login</returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginCommand command, CancellationToken cancellationToken) 
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

