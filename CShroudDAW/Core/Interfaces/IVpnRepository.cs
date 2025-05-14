using Ardalis.Result;
using CShroudDAW.Core.Entities;
using Google.Protobuf;

namespace CShroudDAW.Core.Interfaces;

public interface IVpnRepository
{
    Task<Result> AddKey(VpnProtocol protocol, uint vpnLevel, string email, string credentials);

    Task DelKey(VpnProtocol protocol, string connectionId);
    
    // Task<SysStatsResponse?> GetSysStat();
}