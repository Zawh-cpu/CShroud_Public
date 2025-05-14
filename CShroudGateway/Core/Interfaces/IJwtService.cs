namespace CShroudGateway.Core.Interfaces;

public interface IJwtService
{
    string GenerateJwt(Guid userIdd, string sub, DateTime createdAt, DateTime expiresAt);
}