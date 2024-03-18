using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Models;

namespace EventManagement.Application.Mapping;

public class OrganizerProfile : AutoMapper.Profile
{
    public OrganizerProfile()
    {
        CreateMap<Organizer, OrganizerDto>();
    }
}
