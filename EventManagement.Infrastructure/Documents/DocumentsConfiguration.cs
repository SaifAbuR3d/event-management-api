using EventManagement.Application.Abstractions.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Documents;

public static class DocumentsConfiguration
{
    public static IServiceCollection AddDocumentsHandlingInfrastructure(
    this IServiceCollection services, IConfiguration configuration)
    => services.AddScoped<IDocumentHandler, DocumentHandler>();
}
