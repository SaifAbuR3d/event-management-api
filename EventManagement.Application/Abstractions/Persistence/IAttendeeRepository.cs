using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IAttendeeRepository
{
    Task<Attendee> AddAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Attendee> UpdateAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<Attendee> DeleteAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<(IEnumerable<Attendee>, PaginationMetadata)> GetAttendeesFollowingAnOrganizerAsync(int organizerId,
    GetAllQueryParameters parameters, CancellationToken cancellationToken);
}
