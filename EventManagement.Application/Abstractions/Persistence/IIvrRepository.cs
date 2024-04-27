using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IIvrRepository
{
    Task<IdentityVerificationRequest> AddAsync(IdentityVerificationRequest ivr,
        CancellationToken cancellationToken);
    Task<IdentityVerificationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IdentityVerificationRequest?> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<(IEnumerable<IdentityVerificationRequest>, PaginationMetadata)> GetAllAsync(
        IdentityVerificationRequestStatus status, GetAllIvrsQueryParameters queryParameters,
        CancellationToken cancellationToken);
    Task<bool> HasPendingRequests(int userId, CancellationToken cancellationToken);
}
