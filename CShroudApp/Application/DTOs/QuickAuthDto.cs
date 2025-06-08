using System.Text.Json.Serialization;

namespace CShroudApp.Application.DTOs;

[JsonConverter(typeof(JsonStringEnumConverter))]
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