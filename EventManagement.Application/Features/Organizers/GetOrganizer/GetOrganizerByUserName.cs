using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Organizers.GetOrganizer;

public record GetOrganizerByUserName(string UserName) : IRequest<OrganizerDto>;

public class GetOrganizerByUsernameHandler(IUserRepository userRepository, 
    IOrganizerRepository organizerRepository, IMapper mapper)
    : IRequestHandler<GetOrganizerByUserName, OrganizerDto>
{
    public async Task<OrganizerDto> Handle(GetOrganizerByUserName request, CancellationToken cancellationToken)
    {
        var organizer = await organizerRepository.GetOrganizerByUserNameAsync(request.UserName, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), nameof(request.UserName), request.UserName);

        var organizerDto = mapper.Map<OrganizerDto>(organizer);

        organizerDto.ImageUrl = await userRepository.GetProfilePictureByUserId(organizer.UserId,
            cancellationToken);
        organizerDto.UserName = request.UserName;

        return organizerDto;
    }
}
