using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IRegRequestRepository
{
    Task<RegistrationRequest> AddAsync(RegistrationRequest registrationRequest,
        CancellationToken cancellationToken);
    Task<(IEnumerable<RegistrationRequest>, PaginationMetadata)> GetAllAsync(int eventId, GetAllRegRequestQueryParameters queryParameters, CancellationToken cancellationToken);
    Task<RegistrationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
