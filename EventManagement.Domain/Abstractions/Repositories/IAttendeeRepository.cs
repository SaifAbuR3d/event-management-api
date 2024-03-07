using EventManagement.Domain.Models;

namespace EventManagement.Domain.Abstractions.Repositories;

public interface IAttendeeRepository
{
    Task<Attendee> AddAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Attendee> UpdateAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<Attendee> DeleteAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
}
