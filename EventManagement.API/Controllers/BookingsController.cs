using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Bookings.CheckIn;
using EventManagement.Application.Features.Bookings.GetAllBookings;
using EventManagement.Application.Features.Events.GetAllEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

    /// <summary>
    /// Validate a Ticket
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="checkInCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("events/{eventId}/tickets/{checkInCode}")]
    public async Task<ActionResult> CheckIn(int eventId, Guid checkInCode,
               CancellationToken cancellationToken)
    {
        var command = new CheckInCommand(eventId, checkInCode);
        await mediator.Send(command, cancellationToken);
        return Ok(new { message = "Operation Successful" });
    }
    /// <summary>
    /// get all bookings for an event, filtered by attendeeId if provided, sorted and paginated
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpGet("events/{eventId}/bookings")]
    public async Task<ActionResult<(IEnumerable<BookingDto>, PaginationMetadata )>> GetBookings(
        int eventId,[FromQuery] GetAllBookingsQueryParameters parameters, CancellationToken cancellationToken)
    {
        var (tickets, paginationMetadata) = await mediator.Send(
            new GetAllBookingsQuery(eventId, parameters), cancellationToken);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(tickets);
    }

}
