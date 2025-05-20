using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Entities.Protocols;

public class VlessSettings
{
    public required string Flow { get; set; }
    public required string ServerName { get; set; }
    public required bool Insecure { get; set; }
    public required string PublicKey { get; set; }
    public required string ShortId { get; set; }
}