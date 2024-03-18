using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Models;

namespace EventManagement.Application.Mapping;

public class EventProfile : AutoMapper.Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ThumbnailUrl,
                opt => opt.MapFrom(src => src.EventImages.First(ei => ei.IsThumbnail).ImageUrl))
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.EventImages.Where(ei => !ei.IsThumbnail).Select(ei => ei.ImageUrl))); 
    }
}
