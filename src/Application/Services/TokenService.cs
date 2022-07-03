using Application.IServices;
using Application.Options;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;
public class TokenService : ITokenService
{
    private readonly TokenSecretOptions _options;

    public TokenService(IOptions<TokenSecretOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(User user)
    {
        Log.Information($"Generating Token for user {user.Name}");

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.ASCII.GetBytes(_options.Secret);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            })
        };

        var token = jwtTokenHandler.CreateToken(descriptor);
        return jwtTokenHandler.WriteToken(token);
    }
}
