using AutoMapper;
using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Responses;
using MediatR;

namespace EventManagement.Application.Features.Categories.GetCategories;

public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetCategoriesAsync(cancellationToken);
        return mapper.Map<List<CategoryDto>>(categories);
    }
}