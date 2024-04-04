using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class BookingProfile : AutoMapper.Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.BookingTickets));
    }
}
