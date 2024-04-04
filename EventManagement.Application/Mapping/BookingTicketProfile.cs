using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class BookingTicketProfile : AutoMapper.Profile
{
    public BookingTicketProfile()
    {
        CreateMap<BookingTicket, PersonTicketDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Ticket.Price))
            .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.Ticket.Name))
            .ForMember(dest => dest.TicketTypeId, opt => opt.MapFrom(src => src.Ticket.Id));
    }
}
