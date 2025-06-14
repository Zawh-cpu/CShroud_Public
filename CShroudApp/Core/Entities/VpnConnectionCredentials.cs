using System.Text.Json.Nodes;

namespace CShroudApp.Core.Entities;

public class VpnConnectionCredentials
{
    public required VpnProtocol Protocol { get; set; }
    public required string Location { get; set; }
    
    public string? IpAddressV4 { get; set; }
    public string? IpAddressV6 { get; set; }
    
    public required string Host { get; set; }
    public required uint Port { get; set; }
    
    public required List<string> TransparentHosts { get; set; }
    
    public required string YourIPv4Address { get; set; }
    
    public required DateTime Obtained { get; set; }
    public required JsonObject Credentials { get; set; }
}