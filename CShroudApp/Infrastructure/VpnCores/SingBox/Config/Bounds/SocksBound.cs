namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

public class SocksBound : BoundObject
{
    public new string Type { get; set; }= "socks";
    
    public required string Listen { get; set; }
    public required uint ListenPort { get; set; }
    public bool Sniff { get; set; } = false;
    public bool SniffOverrideDestination { get; set; } = false;
}