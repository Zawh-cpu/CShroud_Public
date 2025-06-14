namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

public class HttpBound : BoundObject
{
    public new string Type = "http";
    
    public required string Listen { get; set; }
    public required uint ListenPort { get; set; }
    public bool Sniff { get; set; } = false;
    public bool SniffOverrideDestination { get; set; } = false;
}