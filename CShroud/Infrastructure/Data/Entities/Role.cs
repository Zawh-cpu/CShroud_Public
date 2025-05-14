using System.ComponentModel.DataAnnotations;

namespace CShroud.Infrastructure.Data.Entities;


public class Role
{
    [Key]
    public uint Id { get; set; }
    
    [MaxLength(24)]
    public string? Name { get; set; }

    public int RoleLevel { get; set; } = 0;

    public bool AdminAccess { get; set; } = false;
}