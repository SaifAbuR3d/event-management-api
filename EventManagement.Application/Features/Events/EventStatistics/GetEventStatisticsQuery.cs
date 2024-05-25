using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Events.EventStatistics;

public record GetEventStatisticsQuery(int EventId) : IRequest<EventStatisticsDto>; 

public class GetEventStatisticsQueryHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    IEventRepository eventRepository,
    ITicketRepository ticketRepository, 
    IBookingRepository bookingRepository,
    IMapper mapper)
    : IRequestHandler<GetEventStatisticsQuery, EventStatisticsDto>
{

    public async Task<EventStatisticsDto> Handle(GetEventStatisticsQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        // await Authorize(@event, cancellationToken);

        var ticketDtos = await GetTicketsStats(@event, cancellationToken);

        var lastTransactions = await GetLastTransactions(@event, cancellationToken);

        var sellingTrack = await ticketRepository.GetSellingTrack(@event.Id, cancellationToken);


        var statistics = new EventStatisticsDto(ticketDtos, lastTransactions, sellingTrack, @event.IsManaged);

        return statistics;
    }

    private async Task<List<AttendeeTransaction>> GetLastTransactions(Event @event, CancellationToken cancellationToken)
    {
        var lastBookings = await bookingRepository.GetLastBookings(@event.Id, 3, cancellationToken);
        List<AttendeeTransaction> lastTransactions = [];
        foreach (var booking in lastBookings)
        {
            var attendee = booking.Attendee;

            var transaction = new AttendeeTransaction(attendee.Id,
                await userRepository.GetUserNameByUserId(attendee.UserId, cancellationToken)
                    ?? throw new CustomException("User Has No UserName"),
                await userRepository.GetFullNameByUserId(attendee.UserId, cancellationToken)
                    ?? throw new CustomException("User Has No FullName"),
                await userRepository.GetProfilePictureByUserId(attendee.UserId, cancellationToken),
                booking.BookingTickets.Count,
                booking.CreationDate);

            lastTransactions.Add(transaction);
        }

        return lastTransactions;
    }

    private async Task<IEnumerable<TicketDto>> GetTicketsStats(Event @event, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository.GetEventTickets(@event.Id, cancellationToken);
        var ticketDtos = mapper.Map<IEnumerable<TicketDto>>(tickets);
        return ticketDtos;
    }

    private async Task Authorize(Event @event, CancellationToken cancellationToken)
    {
        var currentUserId = int.Parse(await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
            ?? throw new CustomException($"No User With UserId {currentUser.UserId}"));

        if (currentUserId != @event.OrganizerId)
        {
            throw new UnauthorizedException("You are not authorized to view this event statistics");
        }
    }
}
