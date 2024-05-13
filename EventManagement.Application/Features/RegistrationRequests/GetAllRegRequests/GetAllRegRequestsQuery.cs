using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.RegistrationRequests.GetAllRegRequests;

public record GetAllRegRequestsQuery(int EventId, GetAllRegRequestQueryParameters Parameters)
    : IRequest<(IEnumerable<RegRequestDto>, PaginationMetadata)>;

public class GetAllRegRequestsQueryHandler(ICurrentUser currentUser,
    IEventRepository eventRepository,
    IRegRequestRepository repository,
    IUserRepository userRepository)
    : IRequestHandler<GetAllRegRequestsQuery, (IEnumerable<RegRequestDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<RegRequestDto>, PaginationMetadata)> Handle(
               GetAllRegRequestsQuery request, CancellationToken cancellationToken)
    {
       // await AuthorizeCurrentUserAccess(request, cancellationToken);

        var (regRequests, paginationMetadata) = await repository.GetAllAsync(request.EventId,
            request.Parameters, cancellationToken);

        List<RegRequestDto> regRequestDtos = [];

        foreach (var regRequest in regRequests)
        {
            var regRequestDto = new RegRequestDto
            {
                Id = regRequest.Id,
                AttendeeId = regRequest.AttendeeId,
                AttendeeUserName = await userRepository.GetUserNameByUserId(regRequest.Attendee.UserId,
                   cancellationToken) ?? throw new CustomException("Invalid State: User Has No UserName"),
                AttendeeProfilePictureUrl = await userRepository.GetProfilePictureByUserId(
                    regRequest.Attendee.UserId, cancellationToken),
                Status = regRequest.Status,
                EventId = regRequest.EventId,
                CreationDate = regRequest.CreationDate,
                LastModified = regRequest.LastModified
            };

            regRequestDtos.Add(regRequestDto);
        }

        return (regRequestDtos, paginationMetadata);
    }

    private async Task AuthorizeCurrentUserAccess(GetAllRegRequestsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsOrganizer)
        {
            throw new UnauthorizedException("Only organizers can view registration requests");
        }

        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(Event), request.EventId);

        var requestedEventOrganizerId = int.Parse(await userRepository.GetIdByUserId(currentUser.UserId,
            cancellationToken) ?? throw new CustomException("Invalid State: User Has No Id"));

        if (@event.OrganizerId != requestedEventOrganizerId)
        {
            throw new UnauthorizedException("You are not authorized to view registration requests for this event");
        }
    }
}