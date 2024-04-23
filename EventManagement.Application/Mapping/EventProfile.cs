using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class EventProfile : AutoMapper.Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ThumbnailUrl,
                opt => opt.MapFrom(src => src.EventImages.First(ei => ei.IsThumbnail).ImageUrl))
            .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom(src => src.EventImages.Where(ei => !ei.IsThumbnail).Select(ei => ei.ImageUrl)))
            .ForMember(dest => dest.Lat,
                opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Lon,
                opt => opt.MapFrom(src => src.Longitude))

            .ForMember(dest => dest.HasStarted, opt => opt.MapFrom(src => src.HasStarted()))
            .ForMember(dest => dest.HasEnded, opt => opt.MapFrom(src => src.HasEnded()))
            .ForMember(dest => dest.IsRunning, opt => opt.MapFrom(src => src.IsRunning()))
            .ForMember(dest => dest.TicketsSalesStarted, opt => opt.MapFrom(src => src.TicketsSalesStarted()))
            .ForMember(dest => dest.TicketsSalesEnded, opt => opt.MapFrom(src => src.TicketsSalesEnded()));

    }
}
