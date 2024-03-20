using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Domain.Entities;
using MediatR;

namespace EventManagement.Application.Features.Organizers.GetOrganizer;

public record GetOrganizerByIdQuery(int OrganizerId) : IRequest<OrganizerDto>;

public class GetOrganizerByIdQueryHandler(IOrganizerRepository organizerRepository, IMapper mapper)
    : IRequestHandler<GetOrganizerByIdQuery, OrganizerDto>
{
    public async Task<OrganizerDto> Handle(GetOrganizerByIdQuery request, CancellationToken cancellationToken)
    {
        var organizer = await organizerRepository.GetOrganizerByIdAsync(request.OrganizerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Organizer), request.OrganizerId);

        return mapper.Map<OrganizerDto>(organizer);
    }
}