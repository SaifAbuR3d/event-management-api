using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

/// <summary>
/// Endpoints for making/managing bookings
/// </summary>
[ApiController]
[Route("api")]
public class BookingsController(IMediator mediator,
    IWebHostEnvironment environment) : ControllerBase
{

    /// <summary>
    /// Create a new booking for an event for the current authenticated user
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpPost("events/{eventId}/bookings")]
    public async Task<ActionResult<BookingDto>> CreateBooking(int eventId, CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(eventId, environment.WebRootPath);
        var booking = await mediator.Send(command, cancellationToken);
        return Ok(booking);
    }


}
