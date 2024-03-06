using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistenceInfrastructure(
        this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        => services
            .AddDatabase(configuration.GetConnectionString("SqlServer"), isDevelopment);

    private static IServiceCollection AddDatabase(
    this IServiceCollection services, string connectionString, bool isDevelopment)
    => services
        .AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(connectionString)
               .EnableSensitiveDataLogging(isDevelopment)
               .EnableDetailedErrors(isDevelopment));

}