using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CShroudGateway.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly AuthConfig _authConfig;
    private readonly SigningCredentials _signingCredentials;

    public JwtService(IOptions<AuthConfig> authConfig)
    {
        _authConfig = authConfig.Value;
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_authConfig.SecretKey), SecurityAlgorithms.HmacSha256);
    }
    
    public string GenerateJwt(Guid tokenId, string sub, DateTime createdAt, DateTime expiresAt)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, tokenId.ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(createdAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Sub, sub),
            new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiresAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _authConfig.JwtIssuer,
            audience: _authConfig.JwtAudience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: _signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}