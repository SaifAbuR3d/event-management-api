using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Organizers.GetOrganizerFollowers;

public record GetOrganizerFollowersQuery(int OrganizerId)
    : IRequest<(IEnumerable<AttendeeDto>, PaginationMetadata)>;

public class GetOrganizerFollowersQueryHandler (IOrganizerRepository organizerRepository,
    IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetOrganizerFollowersQuery, (IEnumerable<AttendeeDto>, PaginationMetadata)>
{

    public async Task<(IEnumerable<AttendeeDto>, PaginationMetadata)> Handle(GetOrganizerFollowersQuery request, CancellationToken cancellationToken)
    {
        var (followers, paginationMetadata) = await organizerRepository.GetFollowersByOrganizerIdAsync(request.OrganizerId);

        var followerDtos = mapper.Map<IEnumerable<AttendeeDto>>(followers);

        foreach(var follower in followerDtos)
        {
            follower.FullName = await userRepository.GetFullNameByUserId(follower.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), follower.UserId);

            follower.UserName = await userRepository.GetUserNameByUserId(follower.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Attendee), nameof(Attendee.UserId), follower.UserId);
        }

        return (followerDtos, paginationMetadata);
    }
}
