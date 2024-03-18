using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Models;
namespace EventManagement.Application.Mapping;

public class CityProfile : AutoMapper.Profile
{
    public CityProfile()
    {
        CreateMap<City, CityDto>();
    }
}
