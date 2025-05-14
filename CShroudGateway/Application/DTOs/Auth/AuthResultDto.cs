namespace CShroudGateway.Application.DTOs.Auth;

public record AuthResultDto
{
    public required Guid UserId { get; init; }
    public required string ActionToken { get; init; }
    public required string RefreshToken { get; init; }
}