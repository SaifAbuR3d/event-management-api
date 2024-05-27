using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using MediatR;

namespace EventManagement.Application.Features.Attendees.GetAttendees;

public record GetAttendeesQuery(GetAllAttendeesQueryParameters Parameters)
    : IRequest<(IEnumerable<AttendeeDto>, PaginationMetadata)>;

public class GetAttendeesQueryHandler(IAttendeeRepository attendeeRepository, 
    IUserRepository userRepository)
    : IRequestHandler<GetAttendeesQuery, (IEnumerable<AttendeeDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<AttendeeDto>, PaginationMetadata)> Handle(GetAttendeesQuery request, CancellationToken cancellationToken)
    {
        var (attendees, paginationMetadata) = await attendeeRepository.GetAttendeesAsync(
            request.Parameters, cancellationToken);

        List<AttendeeDto> attendeeDtos = []; 

        foreach(var attendee in attendees)
        {
            var attendeeDto = new AttendeeDto
            {
                Id = attendee.Id,
                UserId = attendee.UserId,
                IsVerified = attendee.IsVerified,
                Gender = attendee.Gender,
                UserName = await userRepository.GetUserNameByUserId(attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee Has No UserName"),
                FullName = await userRepository.GetFullNameByUserId(attendee.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Attendee Has No FullName"),
                ImageUrl = await userRepository.GetProfilePictureByUserId(attendee.UserId, cancellationToken),
                DateOfBirth = attendee.DateOfBirth,
            }; 
            attendeeDtos.Add(attendeeDto);
        }

        return (attendeeDtos, paginationMetadata);

    }
}
