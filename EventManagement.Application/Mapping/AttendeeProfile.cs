using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class AttendeeProfile : AutoMapper.Profile
{
    public AttendeeProfile()
    {
        CreateMap<Attendee, AttendeeDto>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore());
    }
}
