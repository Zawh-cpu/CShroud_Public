namespace CShroudGateway.Presentation.Api.v1.Responses;

public class FastLoginInfoResponse
{
    public required Guid Id { get; set; }
    public required uint[] Variants { get; set; }
}