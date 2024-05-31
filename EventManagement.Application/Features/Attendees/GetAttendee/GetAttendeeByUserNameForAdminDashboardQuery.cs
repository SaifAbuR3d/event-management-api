using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Application.Features.Attendees.GetAttendee;

public record GetAttendeeByUserNameForAdminDashboardQuery(string UserName) : IRequest<AttendeeDto>;

public class GetAttendeeByUserNameForAdminDashboardQueryHandler(IServiceScopeFactory scopeFactory, 
    ICurrentUser currentUser, IUserRepository userRepository)
    : IRequestHandler<GetAttendeeByUserNameForAdminDashboardQuery, AttendeeDto>
{
    public async Task<AttendeeDto> Handle(GetAttendeeByUserNameForAdminDashboardQuery request,
        CancellationToken cancellationToken)
    {
        if(!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only Admins can access this resource");
        }

        using var scope = scopeFactory.CreateScope();
        var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
        var attendeeDto = await mediatr.Send(new GetAttendeeByUserNameQuery(request.UserName), 
            cancellationToken);

        attendeeDto.Email = await userRepository.GetEmailByUserName(request.UserName,
            cancellationToken);

        return attendeeDto; 
    }
}