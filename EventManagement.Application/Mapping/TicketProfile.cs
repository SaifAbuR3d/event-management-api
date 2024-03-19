using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class TicketProfile : AutoMapper.Profile
{
    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>();
    }
}
