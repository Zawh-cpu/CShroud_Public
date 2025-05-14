using Ardalis.Result;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IVpnRepository
{
    Task<Result<object>> AddKey(Server server, Guid keyId, VpnProtocol protocol, uint vpnLevel, string options);
    Task DelKey(Server server, Guid keyId, VpnProtocol protocol);
}