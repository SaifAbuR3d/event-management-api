using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class OrganizerProfile : AutoMapper.Profile
{
    public OrganizerProfile()
    {
        CreateMap<Organizer, OrganizerDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
    }
}
