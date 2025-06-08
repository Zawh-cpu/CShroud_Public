namespace CShroudApp.Core.Entities.User;

public struct Role
{
    public uint Id { get; set; }
    
    public string? Name { get; set; }

    public int RoleLevel { get; set; }

    // 00000000 00000000 00000000 00000000
    //                                   1 - Admin Rights
    public UInt32 Permissions { get; set; }
}