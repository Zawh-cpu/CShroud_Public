namespace CShroudApp.Application.DTOs;

public class ActionTokenRefreshDto
{
    public required Guid UserId { get; set; }
    public required string ActionToken { get; set; }
}