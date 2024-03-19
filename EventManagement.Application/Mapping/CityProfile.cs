using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mapping;

public class CityProfile : AutoMapper.Profile
{
    public CityProfile()
    {
        CreateMap<City, CityDto>();
    }
}
