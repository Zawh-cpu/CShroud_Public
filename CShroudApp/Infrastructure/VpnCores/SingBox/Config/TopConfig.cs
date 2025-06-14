using System.Text.Json.Serialization;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

public class TopConfig
{
    public LogObject Log { get; set; } = new();
    public DnsObject Dns { get; set; } = new();
    public Dictionary<string, object> Ntp { get; set; } = new();
    public Dictionary<string, object> Certificate { get; set; } = new();
    
    public List<Dictionary<string, object>> Endpoints { get; set; } = new();
    public List<BoundObject> Inbounds { get; set; } = new();
    public List<BoundObject> Outbounds { get; set; } = new();
    
    public RouteObject Route { get; set; } = new();
    public Dictionary<string, object> Experimental { get; set; } = new();
    
    [JsonExtensionData]
    public Dictionary<string, object> External { get; set; } = new();
}