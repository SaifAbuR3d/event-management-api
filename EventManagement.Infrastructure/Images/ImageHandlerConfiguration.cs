using EventManagement.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Images;

public static class ImageHandlerConfiguration
{
    public static IServiceCollection AddImageHandlingInfrastructure(
    this IServiceCollection services, IConfiguration configuration)
    => services.AddScoped<IImageHandler, ImageHandler>();

}
