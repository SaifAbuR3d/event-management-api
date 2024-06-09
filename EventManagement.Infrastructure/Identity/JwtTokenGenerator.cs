using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagement.Infrastructure.Identity;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;
    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }
    public async Task<string?> GenerateToken(ApplicationUser user, IList<string> roles)
    {
        if (_jwtSettings.Key == null
            || _jwtSettings.Issuer == null)
        {
            throw new TokenGenerationFailedException("JwtSettings section is missing");
        }

        var claims = await GetClaims(user, roles);

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.TokenExpirationInHours),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

        var encryptedToken = new JwtSecurityTokenHandler()
           .WriteToken(token);

        return encryptedToken;
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user, IList<string> roles)
    {
        if (user.Email == null || user.UserName == null || roles.IsNullOrEmpty())
        {
            throw new UnauthenticatedException();
        }

        var id = await _userRepository.GetIdByUserId(user.Id, CancellationToken.None)
            ?? throw new CustomException("Invalid State: User Id Not Found");
        bool isVerified = await _userRepository.IsVerified(user.Id, CancellationToken.None); 
        var userImage = await _userRepository.GetProfilePictureByUserId(user.Id, CancellationToken.None);

        var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                    new("id", id ), 
                    new("isVerified", isVerified.ToString()),
                    new("userImage", userImage ?? "")
                };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
