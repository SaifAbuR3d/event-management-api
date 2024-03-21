using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Follow.GetOrganizerFollowers;

public record GetAttendeesFollowingAnOrganizerQuery(int OrganizerId, GetAllQueryParameters parameters)
    : IRequest<(IEnumerable<AttendeeDto>, PaginationMetadata)>;

public class GetAttendeesFollowingAnOrganizerQueryHandler(IAttendeeRepository attendeeRepository,
    IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetAttendeesFollowingAnOrganizerQuery, (IEnumerable<AttendeeDto>, PaginationMetadata)>
{

    public async Task<(IEnumerable<AttendeeDto>, PaginationMetadata)> Handle(GetAttendeesFollowingAnOrganizerQuery request, CancellationToken cancellationToken)
    {
        var (followers, paginationMetadata) = await attendeeRepository.GetAttendeesFollowingAnOrganizerAsync(request.OrganizerId,
            request.parameters, cancellationToken);

        var followerDtos = mapper.Map<IEnumerable<AttendeeDto>>(followers);

        foreach (var follower in followerDtos)
        {
            follower.FullName = await userRepository.GetFullNameByUserId(follower.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), follower.UserId);

            follower.UserName = await userRepository.GetUserNameByUserId(follower.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), follower.UserId);
        }

        return (followerDtos, paginationMetadata);
    }
}
