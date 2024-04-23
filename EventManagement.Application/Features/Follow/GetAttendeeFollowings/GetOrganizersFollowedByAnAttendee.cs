using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using MediatR;

namespace EventManagement.Application.Features.Follow.GetFollowings;

public record GetOrganizersFollowedByAnAttendee(int AttendeeId,
    GetAttendeeFollowingsQueryParameters Parameters)
    : IRequest<(IEnumerable<OrganizerDto>, PaginationMetadata)>;

public class GetOrganizersFollowedByAnAttendeeHandler(IOrganizerRepository organizerRepository,
    IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetOrganizersFollowedByAnAttendee, (IEnumerable<OrganizerDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<OrganizerDto>, PaginationMetadata)> Handle(GetOrganizersFollowedByAnAttendee request,
        CancellationToken cancellationToken)
    {
        var (organizers, paginationMetadata) = await organizerRepository
            .GetOrganizersFollowedByAttendee(request.AttendeeId, request.Parameters, cancellationToken);

        var organizerDtos = mapper.Map<IEnumerable<OrganizerDto>>(organizers);
        foreach (var organizer in organizerDtos)
        {
            organizer.UserName = await userRepository.GetUserNameByUserId(organizer.UserId, cancellationToken)
                ?? throw new CustomException("Invalid State: Organizer has no UserName"); 
        }

        return (organizerDtos, paginationMetadata);
    }
}
