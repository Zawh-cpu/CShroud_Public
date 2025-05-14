using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Infrastructure.Data.Entities;


public class Role
{
    [Key]
    public uint Id { get; set; }
    
    [MaxLength(24)]
    public string? Name { get; set; }

    public int RoleLevel { get; set; } = 0;

    // 00000000 00000000 00000000 00000000
    //                                   1 - Admin Rights
    public UInt32 Permissions { get; set; } = 0;
}