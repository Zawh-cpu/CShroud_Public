namespace CShroudGateway.Presentation.Api.v1.Responses;

public class SignInResponse
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
    public required string ActionToken { get; init; }
}