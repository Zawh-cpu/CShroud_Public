namespace CShroudGateway.Presentation.Api.v1.Responses;

public class FastLoginStartResponse
{
    public required Guid Id { get; set; }
    public required uint ValidVariant { get; set; }
    public required string Secret { get; set; }
}