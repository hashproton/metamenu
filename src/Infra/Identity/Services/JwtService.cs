using Infra.Identity.Configuration;
using Infra.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infra.Identity.Services;

public interface IJwtService
{
    Task<JwtAccessToken> GenerateAsync(User user);
}

public class JwtService(
    IOptions<IdentityConfiguration> options,
    UserManager<User> userManager) : IJwtService
{
    public async Task<JwtAccessToken> GenerateAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(options.Value.SecretKey);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new JwtAccessToken { Token = tokenHandler.WriteToken(token) };
    }
}