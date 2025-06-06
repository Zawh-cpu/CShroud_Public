﻿using Newtonsoft.Json.Linq;

namespace CShroudApp.Core.Entities.Vpn.Bounds;

public class Http : IVpnBound
{
    public string Tag { get; set; }
    public VpnProtocol Type { get; set; } = VpnProtocol.Http;
    public string Host { get; set; }
    public uint Port { get; set; }
    public bool Sniff { get; set; }
    public bool SniffOverrideDestination { get; set; }
}