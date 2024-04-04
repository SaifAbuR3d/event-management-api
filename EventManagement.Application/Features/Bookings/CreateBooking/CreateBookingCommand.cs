using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Abstractions.QrCode;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using FluentValidation;
using MediatR;

namespace EventManagement.Application.Features.Bookings.CreateBooking;

public record CreateBookingCommand(int EventId, List<RequestedTicket> Tickets, 
    string? Notes, int PaymentMethodId, string BaseUrl) : IRequest<BookingDto>;

public class CreateBookingCommandHandler(IValidator<CreateBookingCommand> validator,
    IUnitOfWork unitOfWork,
    IMapper mapper, ICurrentUser currentUser, IAttendeeRepository attendeeRepository,
    IEventRepository eventRepository, ITicketRepository ticketRepository, 
    IBookingRepository bookingRepository, IQrCodeGenerator qrCodeGenerator)
    : IRequestHandler<CreateBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        if (!currentUser.IsAttendee)
        {
            throw new UnauthorizedAccessException("Only attendees can make bookings");
        }

        var currentUserId = currentUser.UserId;

        var attendee = await attendeeRepository.GetAttendeeByUserIdAsync(currentUserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), "UserId", currentUserId);

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        Booking bookingEntity = null!;

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var booking = await MakeBooking(attendee, @event, request.Tickets, request.BaseUrl, cancellationToken)
                ?? throw new Exception("Failed to create booking");

            bookingEntity = await bookingRepository.AddBookingAsync(booking, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

        }
        catch (Exception)
        {
           await unitOfWork.RollbackTransactionAsync(cancellationToken);
           throw;
        }


        var bookingDto = mapper.Map<BookingDto>(bookingEntity);

        return bookingDto;
    }

    private async Task<Booking?> MakeBooking(Attendee attendee, Event @event,
        List<RequestedTicket> requestedTickets, string baseUrl, CancellationToken cancellationToken)
    {
        // Validate payment

        var bookingTickets = new List<BookingTicket>();
        foreach(var ticketRequest in requestedTickets)
        {
            var ticket = await ValidateTicket(ticketRequest, cancellationToken)
                ?? throw new NotFoundException(nameof(Ticket), ticketRequest.TicketId);

            var numberOfTickets = ticketRequest.Quantity;

            while(numberOfTickets > 0)
            {
                var checkInCode = Guid.NewGuid();

                var qrCodeImageUrl = await qrCodeGenerator.GenerateQrCodeAsync(ticket.Name, ticket.Price,
                    checkInCode.ToString(), baseUrl, cancellationToken);

                var bookingTicket = new BookingTicket(ticket, qrCodeImageUrl, checkInCode);

                bookingTickets.Add(bookingTicket);

                numberOfTickets--; 
            }

        }

        var booking = new Booking(attendee, @event, bookingTickets);

        return booking;
    }

    private async Task<Ticket?> ValidateTicket(RequestedTicket ticketRequest, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetTicketAsync(ticketRequest.TicketId, cancellationToken)
                ?? throw new NotFoundException(nameof(Ticket), ticketRequest.TicketId);

        bool isSaleActive = ticket.StartSale <= DateTime.UtcNow && ticket.EndSale >= DateTime.UtcNow;

        if (!isSaleActive)
        {
            throw new BadRequestException($"Ticket is not available for sale, sales started at {ticket.StartSale} " +
                $"and ended at {ticket.EndSale}");
        }

        try
        {
            ticket.DecreaseAvailableQuantity(ticketRequest.Quantity);
        }
        catch (InvalidOperationException ex)
        {
            throw new BadRequestException($"{ex.Message} TicketID: {ticket.Id}, " +
                $"All Available: {ticket.AvailableQuantity}, Requested: {ticketRequest.Quantity}");
        }

        return ticket;
    }
}