namespace CShroudGateway.Core.Constants;

public static class ReservedUsers
{
    public static readonly Guid System = Guid.Parse("00000000-0000-0000-0000-000000000001");
    
    private static readonly HashSet<Guid> All = new()
    {
        System
    };
    
    public static bool IsReserved(Guid userId) => All.Contains(userId);
}