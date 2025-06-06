namespace CShroudApp.Application.DTOs;

public class QuickAuthSessionDto
{
    public required Guid SessionId { get; set; }
    public required uint ValidVariant { get; set; }
    public required uint[] Variants { get; set; }
}