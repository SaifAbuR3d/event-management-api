using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Attendees.GetAttendee;

public record GetAttendeeByUserNameQuery(string UserName) : IRequest<AttendeeDto>;

public class GetAttendeeByUserNameQueryHandler(IUserRepository userRepository, 
    IAttendeeRepository attendeeRepository, IMapper mapper)
    : IRequestHandler<GetAttendeeByUserNameQuery, AttendeeDto>
{
    public async Task<AttendeeDto> Handle(GetAttendeeByUserNameQuery request,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAttendeeByUserNameAsync(request.UserName, cancellationToken)
            ?? throw new NotFoundException(nameof(Attendee), nameof(request.UserName), request.UserName);

        var attendeeDto = mapper.Map<AttendeeDto>(attendee);

        attendeeDto.UserName = request.UserName;
        attendeeDto.ImageUrl = await userRepository.GetProfilePictureByUserId(attendee.UserId,
            cancellationToken);
        attendeeDto.FullName = await userRepository.GetFullNameByUserId(attendee.UserId, cancellationToken)
            ?? throw new CustomException("Invalid State: Attendee Has No FullName"); 

        return attendeeDto;
    }
}
