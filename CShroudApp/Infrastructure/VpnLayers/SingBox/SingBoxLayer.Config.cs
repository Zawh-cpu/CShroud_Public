using CShroudApp.Core.Entities.Vpn;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public class SingBoxConfig
{
    public LogObject Log { get; set; } = new();
    public DnsObject Dns { get; set; } = new();
    public Dictionary<string, object> Ntp { get; set; } = new();
    public List<object> Endpoints { get; set; } = new();
    public List<BoundObject> Inbounds { get; set; } = new();
    public List<BoundObject> Outbounds { get; set; } = new();
    public RouteObject Route { get; set; } = new();
    public Dictionary<string, object> Experimental { get; set; } = new();

    public class LogObject
    {
        public bool Disabled { get; set; } = false;
        public string Level { get; set; } = "info";
        public string? Output { get; set; }
        public bool Timestamp { get; set; } = true;
    }

    public class DnsObject
    {
        public List<object> Servers { get; set; } = new();
        public List<object> Rules { get; set; } = new();
        public string? Final { get; set; }
        public string? Strategy  { get; set; }
        public bool? DisableCache { get; set; }
        public bool? DisableExpire { get; set; }
        public bool? IndependentCache { get; set; }
        public int? CacheCapacity { get; set; }
        public bool? ReverseMapping { get; set; }
        public string? ClientSubnet { get; set; }
        public Dictionary<string, object>? Fakeip { get; set; }
    }

    public class BoundObject
    {
        public required string Type { get; set; }
        public required string Tag { get; set; }
        
        [JsonExtensionData]
        public JObject Extra { get; set; } = new();
        
    }

    public class RouteObject
    {
        public List<object> Rules { get; set; } = new();
        public List<object> RuleSet { get; set; } = new();
        public string? Final { get; set; }
        public bool? AutoDetectInterface { get; set; }
        public bool? OverrideAndroidVpn { get; set; }
        public string? DefaultInterface { get; set; }
        public int? DefaultMark { get; set; }
        public string? DefaultDomainResolver { get; set; }
        public string? DefaultNetworkStrategy { get; set; }
        public List<object>? DefaultNetworkType { get; set; }
        public List<object>? DefaultFallbackNetworkType { get; set; }
        public string? DefaultFallbackDelay { get; set; }
    }
}