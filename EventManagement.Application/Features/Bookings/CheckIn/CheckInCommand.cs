using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Bookings.CheckIn;

public record CheckInCommand(int eventId, Guid CheckInCode) : IRequest<Unit>;

public class CheckInCommandHandler(ITicketRepository ticketRepository, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<CheckInCommand, Unit>
{
    public async Task<Unit> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        var bookingTicket = await ticketRepository.GetBookingTicketAsync(request.eventId,
            request.CheckInCode, cancellationToken) ?? throw new NotFoundException(
                nameof(BookingTicket), nameof(request.CheckInCode), request.CheckInCode); 

        if (bookingTicket.IsCheckedIn)
        {
            throw new BadRequestException("Ticket already checked in");
        }

        bookingTicket.CheckIn(); 
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
