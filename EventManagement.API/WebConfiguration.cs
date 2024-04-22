using EventManagement.API.Middlewares;
using EventManagement.API.Services;
using EventManagement.Application.Features.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace EventManagement.API;

/// <summary>
/// Register services in the DI container
/// </summary>
public static class WebConfiguration
{
    /// <summary>
    /// Register services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddWeb(
           this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();

        services.AddEndpointsApiExplorer();

        services.AddSwagger();

        services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin",
                options =>
                options
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithExposedHeaders("X-Pagination"));
        });

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services; 
    }

    private static IServiceCollection AddSwagger(
          this IServiceCollection services)
        => services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Event Management API",
                Version = "v1",
                Description = "API endpoints for managing events",
            });

            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
            });

            setup.UseDateOnlyTimeOnlyStringConverters();

            #region include xml comments
            var actionMethodsXmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var actionMethodsXmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, actionMethodsXmlCommentsFile);

            setup.IncludeXmlComments(actionMethodsXmlCommentsFullPath);
            #endregion
        });
}
