namespace CShroud.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;

public class Rate
{
    [Key]
    public uint Id { get; set; }
    
    [MaxLength(24)]
    public string? Name { get; set; }
    
    public Decimal Cost { get; set; }

    public uint VPNLevel { get; set; } = 0;
    
    public int MaxKeys { get; set; } = 0;
    
    public bool IsPrivate { get; set; } = false;
}