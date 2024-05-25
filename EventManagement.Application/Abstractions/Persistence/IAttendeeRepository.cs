using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IAttendeeRepository
{
    Task<Attendee> AddAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<Attendee?> GetAttendeeByUserIdAsync(int userId, CancellationToken cancellationToken,
        bool includeFollowings = false);
    Task<Attendee?> GetAttendeeByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<(IEnumerable<Attendee>, PaginationMetadata)> GetAttendeesFollowingAnOrganizerAsync(int organizerId,
    GetAllQueryParameters parameters, CancellationToken cancellationToken);
    Task<bool> IsFollowingOrganizer(int attendeeId, int organizerId, CancellationToken cancellationToken);
    Task UnfollowAnOrganizer(int attendeeId, int organizerId, CancellationToken cancellationToken);
    Task<bool> HasAttendedEvent(int attendeeId, int eventId, CancellationToken cancellationToken);
    Task<bool> DoesLikeEvent(int attendeeId, int eventId, CancellationToken cancellationToken);
    Task RemoveLikeFromEvent(int attendeeId, int eventId, CancellationToken cancellationToken);
    Task<bool> HasMadeRegRequest(int attendeeId, int eventId, CancellationToken cancellationToken);
    Task<(IEnumerable<Attendee>, PaginationMetadata)> GetAttendeesAsync(GetAllAttendeesQueryParameters parameters, CancellationToken cancellationToken);

    Task<Attendee> UpdateAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
    Task<Attendee> DeleteAttendeeAsync(Attendee attendee, CancellationToken cancellationToken);
}
