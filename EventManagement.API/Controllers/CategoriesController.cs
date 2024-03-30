using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Features.Categories.CreateCategory;
using EventManagement.Application.Features.Categories.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

/// <summary>
/// The controller for the categories endpoints
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/categories")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all categories 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of all categories</returns>

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return Ok(result);
    }
}
