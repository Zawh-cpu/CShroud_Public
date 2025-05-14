namespace CShroudGateway.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class Protocol
{
    [Key] public required string Id { get; set; }

    public uint Port { get; set; }
    
    public required string PublicKey { get; set; }
    
    public required JsonDocument URIArgs { get; set; }

    public bool IsActive { get; set; } = true;

}