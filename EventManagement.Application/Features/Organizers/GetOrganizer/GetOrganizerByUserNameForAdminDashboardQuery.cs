using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Application.Features.Organizers.GetOrganizer;

public record GetOrganizerByUserNameForAdminDashboardQuery(string UserName) : IRequest<OrganizerDto>;

public class GetOrganizerByUserNameForAdminDashboardQueryHandler(IServiceScopeFactory scopeFactory,
    ICurrentUser currentUser, IUserRepository userRepository)
    : IRequestHandler<GetOrganizerByUserNameForAdminDashboardQuery, OrganizerDto>
{
    public async Task<OrganizerDto> Handle(GetOrganizerByUserNameForAdminDashboardQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only Admins can access this resource");
        }

        using var scope = scopeFactory.CreateScope();
        var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
        var organizerDto = await mediatr.Send(new GetOrganizerByUserNameQuery(request.UserName),
            cancellationToken);

        organizerDto.Email = await userRepository.GetEmailByUserName(request.UserName,
            cancellationToken);

        return organizerDto;
    }
}