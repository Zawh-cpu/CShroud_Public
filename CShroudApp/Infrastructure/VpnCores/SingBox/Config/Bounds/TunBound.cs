namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

public class TunBound : BoundObject
{
    public string? InterfaceName  { get; set; }
    public required string[] Address  { get; set; }
    public uint Mtu { get; set; } = 9000;
    public bool? AutoRoute  { get; set; }
    public uint? Iproute2TableIndex  { get; set; }
    public uint? Iproute2RuleIndex  { get; set; }
    public bool? AutoRedirect  { get; set; }
    public string? AutoRedirectInputMark  { get; set; }
    public string? AutoRedirectOutputMark  { get; set; }
    public string[]? LoopbackAddress  { get; set; }
    public bool? StrictRoute  { get; set; }
    public string[]? RouteAddress  { get; set; }
    public string[]? RouteExcludeAddress  { get; set; }
    public string[]? RouteAddressSet  { get; set; }
    public string[]? RouteExcludeAddressSet  { get; set; }
    public bool? EndpointIndependentNat  { get; set; }
    public string? UdpTimeout  { get; set; }
    public string? Stack  { get; set; }
    public bool? Sniff  { get; set; }
}