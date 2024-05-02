using EventManagement.Domain.Entities;

namespace EventManagement.Application.Contracts.Requests;

public class GetAllRegRequestQueryParameters : GetAllQueryParameters
{
    public int? AttendeeId { get; set; }
    public RegistrationStatus? Status { get; set; }
}
