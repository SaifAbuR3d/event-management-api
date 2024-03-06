using EventManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Identity;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityInfrastructure(
               this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                // TODO: Set the password requirements
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services; 
    }
}
