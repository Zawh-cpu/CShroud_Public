using CShroudGateway.Core.Entities;

namespace CShroudGateway.Infrastructure.Data.Config;

public class ProtocolsConfig
{
    public class Protocol
    {
        public uint Port { get; set; }
    }

    public Dictionary<VpnProtocol, Protocol> Connections { get; init; } = new();
}