namespace CShroudApp.Core.Entities.User;

public struct Rate
{
    public uint Id { get; set; }
    public string? Name { get; set; }
    public Decimal Cost { get; set; }
    public uint VpnLevel { get; set; }
    public int MaxKeys { get; set; }
    public uint TrafficSpeedLimit { get; set; }
    public uint MaxConnections { get; set; }
    public bool IsPrivate { get; set; }
}