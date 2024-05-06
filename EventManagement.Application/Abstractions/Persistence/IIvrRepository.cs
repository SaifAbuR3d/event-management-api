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
        GetAllIvrsQueryParameters queryParameters,
        CancellationToken cancellationToken);
    Task<bool> HasPendingRequest(int userId, CancellationToken cancellationToken);
    Task<bool> HasRejectedRequest(int userId, CancellationToken cancellationToken);
    Task<bool> DeleteByUserId(int userId, CancellationToken cancellationToken);
    Task VerifyUserAsync(int userId, CancellationToken cancellationToken);
}
