namespace CShroudApp.Application.DTOs;

public class FastLoginDto
{
    public required Guid Id { get; set; }
    public required uint ValidVariant { get; set; }
    public required string Secret { get; set; }
}