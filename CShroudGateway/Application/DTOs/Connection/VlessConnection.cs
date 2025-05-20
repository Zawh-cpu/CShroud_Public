namespace CShroudGateway.Application.DTOs.Connection;

public class VlessConnection : IConnectionInfo
{
    public required string Host { get; set; }
    public required uint Port { get; set; }
    
    public required Guid Uuid { get; set; }
    public required string Flow { get; set; }
    public required string ServerName { get; set; }
    public required bool Insecure { get; set; }
    public required string PublicKey { get; set; }
    public required string ShortId { get; set; }
}