using EventManagement.Application.Exceptions;
using EventManagement.Application.Identity;
using EventManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        var key = configuration.GetSection(nameof(JwtSettings)).GetSection("Key").Value
                    ?? throw new TokenGenerationFailedException("secret key is missing");
        var keyBytes = Encoding.ASCII.GetBytes(key);

        var issuer = configuration.GetSection(nameof(JwtSettings)).GetSection("Issuer").Value;

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IIdentityManager, IdentityManager>();

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        return services;
    }
}
