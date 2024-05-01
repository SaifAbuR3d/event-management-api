using EventManagement.Domain.Entities;

namespace EventManagement.Application.Abstractions.Persistence;

public interface IRegRequestRepository
{
    Task<RegistrationRequest> AddAsync(RegistrationRequest registrationRequest,
        CancellationToken cancellationToken);
    Task<RegistrationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
