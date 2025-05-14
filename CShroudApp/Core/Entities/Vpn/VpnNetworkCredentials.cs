using Newtonsoft.Json.Linq;

namespace CShroudApp.Core.Entities.Vpn;

public class VpnNetworkCredentials
{
    public required VpnProtocol Protocol { get; set; }
    public required string Location { get; set; }
    public required string IPv4 { get; set; }
    public required string IPv6 { get; set; }
    
    public required string ServerHost { get; set; }
    public required uint ServerPort { get; set; }
    
    public required List<string> TransparentHosts { get; set; }
    
    public required string YourIPv4Address { get; set; }
    
    public required DateTime Obtained { get; set; }
    public required JObject Credentials { get; set; }
}