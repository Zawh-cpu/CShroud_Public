using System.Text.Json.Serialization;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

public class TopConfig
{
    public LogObject? Log { get; set; }
    public DnsObject? Dns { get; set; }
    public Dictionary<string, object>? Ntp { get; set; }
    public Dictionary<string, object>? Certificate { get; set; }
    
    public List<Dictionary<string, object>>? Endpoints { get; set; }
    public List<BoundObject>? Inbounds { get; set; }
    public List<BoundObject>? Outbounds { get; set; }
    
    public RouteObject? Route { get; set; }
    public ExperimentalObject? Experimental { get; set; }
    
    [JsonExtensionData]
    public Dictionary<string, object> External { get; set; } = new();
}