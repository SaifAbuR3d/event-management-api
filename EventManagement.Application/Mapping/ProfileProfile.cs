using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class ProfileProfile : AutoMapper.Profile
{
    public ProfileProfile()
    {
        CreateMap<Profile, ProfileDto>()
            .ForAllMembers(opt => opt.NullSubstitute(string.Empty));
    }
}
