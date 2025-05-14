namespace CShroud.Infrastructure.Data.Entities;

using CShroud.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public uint Id { get; set; }
    
    [MaxLength(96)]
    public string? Nickname { get; set; }
    
    [MaxLength(96)]
    public string? Login { get; set; }
    
    [MaxLength(128)]
    public String? Password { get; set; }
    
    
    public ulong? TelegramId { get; set; }
    
    [Required] public uint RoleId { get; set; } = 1;
    
    [ForeignKey("RoleId")]
    public Role? Role { get; set; }
    
    [Required] public uint RateId { get; set; } = 1;
    
    [ForeignKey("RateId")]
    public Entities.Rate? Rate { get; set; }
    
    public List<Entities.Key> Keys { get; set; } = new();
    
    public DateTime? PayedUntil { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? TelegramJoinedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;

}