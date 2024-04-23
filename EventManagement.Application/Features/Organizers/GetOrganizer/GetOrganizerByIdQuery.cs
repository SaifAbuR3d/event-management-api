using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Organizers.GetOrganizer;

public record GetOrganizerByIdQuery(int OrganizerId) : IRequest<OrganizerDto>;

public class GetOrganizerByIdQueryHandler(IUserRepository userRepository,
    IOrganizerRepository organizerRepository, IMapper mapper)
    : IRequestHandler<GetOrganizerByIdQuery, OrganizerDto>
{
    public async Task<OrganizerDto> Handle(GetOrganizerByIdQuery request, CancellationToken cancellationToken)
    {
        var organizer = await organizerRepository.GetOrganizerByIdAsync(request.OrganizerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), request.OrganizerId);

        var organizerDto = mapper.Map<OrganizerDto>(organizer);

        organizerDto.ImageUrl = await userRepository.GetProfilePictureByUserId(organizer.UserId,
            cancellationToken);
        organizerDto.UserName = await userRepository.GetUserNameByUserId(organizer.UserId,
            cancellationToken) ?? throw new CustomException("Invalid State: userName is null");

        // If the organizer does not have a profile, return an empty profile
        if (organizer.Profile == null)
        {
            organizerDto.Profile = new ProfileDto();
        }

        return organizerDto;

    }
}