using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface ITokenService
{
    Token GenerateJwtToken(Guid userId, TokenType tokenType);
}