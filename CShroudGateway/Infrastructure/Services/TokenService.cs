using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IJwtService _jwtService;
    
    public TokenService(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public Token GenerateJwtToken(Guid userId, TokenType tokenType)
    {
        var timeNow = DateTime.UtcNow;
        var expiryTime = timeNow.Add(tokenType.GetLifetime());
        var tokenId = Guid.NewGuid();
        
        var token = new Token()
        {
            Id = tokenId,
            UserId = userId,
            TokenType = tokenType,
            TokenData = _jwtService.GenerateJwt(tokenId, userId.ToString(), timeNow, expiryTime),
            CreatedAt = timeNow,
            Expiry = expiryTime,
        };
        
        return token;
    }
}