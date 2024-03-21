using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using MediatR;

namespace EventManagement.Application.Features.Follow.GetFollowings;

public record GetOrganizersFollowedByAnAttendee(int AttendeeId, GetAllQueryParameters Parameters)
    : IRequest<(IEnumerable<OrganizerDto>, PaginationMetadata)>;

public class GetOrganizersFollowedByAnAttendeeHandler(IOrganizerRepository organizerRepository,
    IMapper mapper)
    : IRequestHandler<GetOrganizersFollowedByAnAttendee, (IEnumerable<OrganizerDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<OrganizerDto>, PaginationMetadata)> Handle(GetOrganizersFollowedByAnAttendee request,
        CancellationToken cancellationToken)
    {
        var (organizers, paginationMetadata) = await organizerRepository
            .GetOrganizersFollowedByAttendee(request.AttendeeId, request.Parameters, cancellationToken);

        var organizerDtos = mapper.Map<IEnumerable<OrganizerDto>>(organizers);

        return (organizerDtos, paginationMetadata);
    }
}
