using Ardalis.Result;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Key = Microsoft.EntityFrameworkCore.Metadata.Internal.Key;

namespace CShroudGateway.Infrastructure.Services;

public class VpnService : IVpnService
{
    public Dictionary<Guid, List<string>> Connections { get; set; } = new();

    // { UUID: [ServerUUID, ServerUUID, ...] }

    private readonly IBaseRepository _baseRepository;

    private readonly IVpnRepository _vpnRepository;

    public VpnService(IBaseRepository baseRepository, IVpnRepository vpnRepository)
    {
        _baseRepository = baseRepository;
        _vpnRepository = vpnRepository;
    }
}