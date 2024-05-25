using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Organizers.GetOrganizer;

public record GetOrganizerByUserNameQuery(string UserName) : IRequest<OrganizerDto>;

public class GetOrganizerByUsernameHandler(IUserRepository userRepository, 
    IOrganizerRepository organizerRepository, IMapper mapper)
    : IRequestHandler<GetOrganizerByUserNameQuery, OrganizerDto>
{
    public async Task<OrganizerDto> Handle(GetOrganizerByUserNameQuery request, CancellationToken cancellationToken)
    {
        var organizer = await organizerRepository.GetOrganizerByUserNameAsync(request.UserName, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), nameof(request.UserName), request.UserName);

        var organizerDto = mapper.Map<OrganizerDto>(organizer);

        organizerDto.ImageUrl = await userRepository.GetProfilePictureByUserId(organizer.UserId,
            cancellationToken);
        organizerDto.UserName = request.UserName;

        // If the organizer does not have a profile, return an empty profile
        if(organizer.Profile == null)
        {
            organizerDto.Profile = new ProfileDto();
        }

        return organizerDto;
    }
}
