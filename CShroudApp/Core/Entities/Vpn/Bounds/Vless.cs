namespace CShroudApp.Core.Entities.Vpn.Bounds;

public class Vless : IVpnBound
{
    public string Tag { get; set; }
    public VpnProtocol Type { get; set; } = VpnProtocol.Vless;
    public string Host { get; set; }
    public uint Port { get; set; }
    public bool Sniff { get; set; }
    public bool SniffOverrideDestination { get; set; }

    public string? Uuid { get; set; }
    public string? Flow { get; set; }
    public string? PacketEncoding { get; set; }
    public string? ServerName { get; set; }
    public bool? Insecure { get; set; }
    public string? Fingerprint { get; set; }
    public string? PublicKey { get; set; }
    public string? ShortId { get; set; }
}