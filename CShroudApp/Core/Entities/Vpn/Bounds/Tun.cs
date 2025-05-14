namespace CShroudApp.Core.Entities.Vpn.Bounds;

public class Tun : IVpnBound
{
    public string Tag { get; set; }
    public VpnProtocol Type { get; set; } = VpnProtocol.Tun;
    public string Host { get; set; }
    public uint Port { get; set; }
    public bool Sniff { get; set; }
    public bool SniffOverrideDestination { get; set; }

    public string? InterfaceName { get; set; }
    public List<string>? Address { get; set; }
    public int? Mtu { get; set; }
    public bool? AutoRoute { get; set; }
    public int? Iproute2TableIndex { get; set; }
    public int? Iproute2RuleIndex { get; set; }
    public bool? AutoRedirect { get; set; }
    public string? AutoRedirectInputMark { get; set; }
    public string? AutoRedirectOutputMark { get; set; }
    public bool? StrictRoute { get; set; }
    public List<string>? RouteAddress { get; set; }
    public List<string>? RouteExcludeAddress { get; set; }
    public List<string>? RouteAddressSet { get; set; }
    public List<string>? RouteExcludeAddressSet { get; set; }
    public bool? EndpointIndependentNat { get; set; }
    public string? UdpTimeout { get; set; }
    public string? Stack { get; set; }
    public List<string>? IncludeInterface { get; set; }
    public List<string>? ExcludeInterface { get; set; }
    public List<string>? IncludeUid { get; set; }
    public List<string>? IncludeUidRange { get; set; }
    public List<string>? ExcludeUid { get; set; }
    public List<string>? ExcludeUidRange { get; set; }
    public List<string>? IncludeAndroidUser { get; set; }
    public List<string>? IncludePackage { get; set; }
    public List<string>? ExcludePackage { get; set; }
    public List<object>? Platform { get; set; }
}    