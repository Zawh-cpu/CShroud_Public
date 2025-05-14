using Newtonsoft.Json.Linq;

namespace CShroudApp.Core.Entities.Vpn.Bounds;

public interface IVpnBound
{
    public string Tag { get; set; }
    public VpnProtocol Type { get; set; }
    public string Host { get; set; }
    public uint Port { get; set; }

    public bool Sniff { get; set; }
    
    public bool SniffOverrideDestination { get; set; }
    
}