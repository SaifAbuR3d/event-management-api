using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Bookings.GetAllBookings;

public record GetAllBookingsQuery(int EventId, GetAllBookingsQueryParameters Parameters)
    : IRequest<(IEnumerable<BookingDto>, PaginationMetadata)>;

public class GetAllBookingsQueryHandler(ICurrentUser currentUser, 
    IUserRepository userRepository,
    IEventRepository eventRepository,
    IBookingRepository bookingRepository, IMapper mapper) :
    IRequestHandler<GetAllBookingsQuery, (IEnumerable<BookingDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<BookingDto>, PaginationMetadata)> Handle(GetAllBookingsQuery request, 
        CancellationToken cancellationToken)
    {
        await Authenticate(request, cancellationToken);

        var (bookings, paginationMetadata) = await bookingRepository.GetBookingsAsync(request.EventId, request.Parameters, cancellationToken);

        var bookingsDto = mapper.Map<IEnumerable<BookingDto>>(bookings);

        return (bookingsDto, paginationMetadata);
    }

    private async Task Authenticate(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
        {
            throw new UnauthenticatedException("You must be authenticated to view bookings");
        }

        // OrganizerId or AttendeeId or AdminId
        int id = int.Parse(await userRepository.GetIdByUserId(currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), currentUser.UserId));

        if(currentUser.IsAttendee)
        {
            if (request.Parameters.AttendeeId.HasValue)
            {

                if (id != request.Parameters.AttendeeId)
                {
                    throw new UnauthorizedException("You are not authorized to view this attendee's bookings");
                }
            }

            else
            {
                throw new UnauthorizedException("You are not authorized to view other bookings than yours");
            }
        }

        else if(currentUser.IsOrganizer)
        {
            var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
                ?? throw new NotFoundException(nameof(Event), request.EventId);

            bool isOrganizerForTheEvent = @event.OrganizerId == id;

            if (!isOrganizerForTheEvent)
            {
                throw new UnauthorizedException("You cannot view bookings for this Event (You are not the organizer for it)");
            }
        }
    }
}
