using Ardalis.Result;
using CShroudDAW.Core.Entities;
using CShroudDAW.Core.Interfaces;

namespace CShroudDAW.Infrastructure.Services;

public class VpnKeyService : IVpnKeyService
{
    private readonly IVpnRepository _vpnRepository;

    public VpnKeyService(IVpnRepository vpnRepository)
    {
        _vpnRepository = vpnRepository;
    }
    
    public async Task<Result> AddKey(VpnProtocol protocol, uint vpnLevel, string connectionId, string credentials)
    {
        return await _vpnRepository.AddKey(protocol, vpnLevel, connectionId, credentials);
    }

    public async Task RemoveKey(VpnProtocol protocol, string connectionId)
    {
        await _vpnRepository.DelKey(protocol, connectionId);
    }
}