using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class AttendeeRepository(ApplicationDbContext context)
    : IAttendeeRepository
{
    public async Task<Attendee> AddAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        var entry = await context.Attendees.AddAsync(attendee, cancellationToken);
        return entry.Entity;
    }

    public Task<Attendee> DeleteAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Attendee?> GetAttendeeByEmailAsync(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Attendee?> GetAttendeeByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Attendee?> GetAttendeeByUserNameAsync(string userName,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Attendee> UpdateAttendeeAsync(Attendee attendee, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
