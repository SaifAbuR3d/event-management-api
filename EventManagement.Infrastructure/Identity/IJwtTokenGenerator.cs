namespace EventManagement.Infrastructure.Identity;

public interface IJwtTokenGenerator
{
    string? GenerateToken(ApplicationUser user, IList<string> roles);
}
