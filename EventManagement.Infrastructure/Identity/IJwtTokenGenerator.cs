namespace EventManagement.Infrastructure.Identity;

public interface IJwtTokenGenerator
{
    Task<string?> GenerateToken(ApplicationUser user, IList<string> roles);
}
