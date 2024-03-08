using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistenceInfrastructure(
        this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        => services
            .AddDatabase(configuration.GetConnectionString("SqlServer"), isDevelopment)
            .AddRepositories();

    private static IServiceCollection AddDatabase(
    this IServiceCollection services, string connectionString, bool isDevelopment)
    => services
        .AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(connectionString)
               .EnableSensitiveDataLogging(isDevelopment)
               .EnableDetailedErrors(isDevelopment));

    public static IServiceCollection AddRepositories(
    this IServiceCollection services)
    => services
        .AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<IAdminRepository, AdminRepository>()
        .AddScoped<IAttendeeRepository, AttendeeRepository>()
        .AddScoped<IOrganizerRepository, OrganizerRepository>();

    // check if there is any pending migration and apply it
    public static IApplicationBuilder Migrate(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        return app;
    }

}