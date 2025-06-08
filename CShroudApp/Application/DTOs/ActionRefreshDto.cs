namespace CShroudApp.Application.DTOs;

public class ActionRefreshDto
{
    public required Guid UserId { get; set; }
    public required string ActionToken { get; set; }
}