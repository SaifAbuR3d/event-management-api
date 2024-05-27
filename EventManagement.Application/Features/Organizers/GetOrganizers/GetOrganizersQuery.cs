using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;

namespace EventManagement.Application.Features.Organizers.GetOrganizers;
public record  GetOrganizersQuery(GetAllOrganizersQueryParameters Parameters)
        : IRequest<(IEnumerable<OrganizerDto>, PaginationMetadata)>;

public class GetOrganizersQueryHandler(ICurrentUser currentUser, 
    IUserRepository userRepository, 
    IOrganizerRepository organizerRepository)
    : IRequestHandler<GetOrganizersQuery, (IEnumerable<OrganizerDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<OrganizerDto>, PaginationMetadata)> Handle(GetOrganizersQuery request, CancellationToken cancellationToken)
    {
        if(!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only Admins Have Access"); 
        }

        var (organizers, paginationMetadata) = await organizerRepository.GetOrganizersAsync(
                       request.Parameters, cancellationToken);

        List<OrganizerDto> organizerDtos = [];

        foreach (var organizer in organizers)
        {
            var organizerDto = new OrganizerDto
            {
                Id = organizer.Id,
                UserId = organizer.UserId,
                DisplayName = organizer.DisplayName,
                IsVerified = organizer.IsVerified,
                UserName = await userRepository.GetUserNameByUserId(organizer.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: Organizer Has No UserName"),
                ImageUrl = await userRepository.GetProfilePictureByUserId(organizer.UserId, cancellationToken),
            };
            organizerDtos.Add(organizerDto);
        }

        return (organizerDtos, paginationMetadata);
    }
}

