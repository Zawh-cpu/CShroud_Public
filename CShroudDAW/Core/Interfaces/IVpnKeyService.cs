using Ardalis.Result;
using CShroudDAW.Core.Entities;

namespace CShroudDAW.Core.Interfaces;

public interface IVpnKeyService
{
    Task<Result> AddKey(VpnProtocol protocol, uint vpnLevel, string connectionId, string credentials);

    Task RemoveKey(VpnProtocol protocol, string connectionId);
}