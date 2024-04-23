using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IOrganizerRepository
{
    Task<Organizer> AddOrganizerAsync(Organizer organizer, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByIdAsync(int id, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<Organizer?> GetOrganizerByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Organizer> DeleteOrganizerAsync(Organizer organizer, CancellationToken cancellationToken);
    Task<(IEnumerable<Organizer>, PaginationMetadata)> GetOrganizersFollowedByAttendee(int attendeeId,
    GetAttendeeFollowingsQueryParameters parameters, CancellationToken cancellationToken);
}
