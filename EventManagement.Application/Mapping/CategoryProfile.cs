using EventManagement.Application.Contracts.Responses;
using EventManagement.Domain.Models;

namespace EventManagement.Application.Mapping;

public class CategoryProfile : AutoMapper.Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
