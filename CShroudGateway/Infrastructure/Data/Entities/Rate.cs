namespace CShroudGateway.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;

public class Rate
{
    [Key]
    public uint Id { get; set; }
    
    [MaxLength(24)]
    public string? Name { get; set; }
    
    public Decimal Cost { get; set; }

    public uint VpnLevel { get; set; } = 0;
    
    public int MaxKeys { get; set; } = 0;
    
    public uint TrafficSpeedLimit { get; set; } = 0;
    public uint MaxConnections { get; set; } = 0;
    
    public bool IsPrivate { get; set; } = false;
}