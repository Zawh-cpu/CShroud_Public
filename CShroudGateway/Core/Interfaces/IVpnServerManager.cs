using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IVpnServerManager
{
    Task <Server?> GetAvailableServerAsync(string location, VpnProtocol protocol);
}