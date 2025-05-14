using CShroudGateway.Core.Interfaces;

namespace CShroudGateway.Infrastructure.Services;

public class VpnStorage : IVpnStorage
{
    public Dictionary<Guid, List<string>> Connections { get; set; } = new();
}