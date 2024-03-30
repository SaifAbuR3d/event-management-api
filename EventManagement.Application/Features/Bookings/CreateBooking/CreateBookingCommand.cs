using EventManagement.Application.Contracts.Responses;
using MediatR;

namespace EventManagement.Application.Features.Bookings.CreateBooking;

public record CreateBookingCommand() : IRequest<BookingDto>;
