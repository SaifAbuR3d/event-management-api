﻿using EventManagement.Application.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagement.Infrastructure.Identity;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public string? GenerateToken(ApplicationUser user, IList<string> roles)
    {
        if (_jwtSettings.Key == null
            || _jwtSettings.Issuer == null)
        {
            throw new TokenGenerationFailedException("JwtSettings section is missing");
        }

        var claims = GetClaims(user, roles);

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

    private static List<Claim> GetClaims(ApplicationUser user, IList<string> roles)
    {
        if (user.Email == null || user.UserName == null || roles.IsNullOrEmpty())
        {
            throw new UnauthenticatedException();
        }

        var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email)
                };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
