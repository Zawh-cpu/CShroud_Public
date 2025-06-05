namespace CShroudApp.Application.DTOs;

public enum QuickAuthStatus
{
    Pending,
    Confirmed,
    Declined,
    Used
}

public class QuickAuthDto
{
    public QuickAuthStatus Status { get; set; }
    public Guid SessionId { get; set; }
    public string SecretLoginCode { get; set; } = string.Empty;
}