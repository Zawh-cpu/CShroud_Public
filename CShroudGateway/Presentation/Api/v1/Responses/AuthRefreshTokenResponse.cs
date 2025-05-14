namespace CShroudGateway.Presentation.Api.v1.Responses;

public class AuthRefreshTokenResponse
{
    public required Guid UserId { get; set; }
    public required string ActionToken { get; set; }
}