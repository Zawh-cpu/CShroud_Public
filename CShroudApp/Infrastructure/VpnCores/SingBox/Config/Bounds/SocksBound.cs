namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

public class SocksBound : BoundObject
{
    public required string Listen { get; set; }
    public required uint ListenPort { get; set; }
    public bool? Sniff { get; set; }
    public bool? SniffOverrideDestination { get; set; }
}