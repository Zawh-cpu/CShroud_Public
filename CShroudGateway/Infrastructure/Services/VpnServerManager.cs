using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Services;

public class VpnServerManager : IVpnServerManager
{
    private readonly IBaseRepository _baseRepository;

    public VpnServerManager(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }
    
    public async Task<Server?> GetAvailableServerAsync(string location, VpnProtocol protocol)
    {
        //var servers = await _baseRepository.GetS
        var servers = await _baseRepository.GetServersByLocationAndProtocolsAsync(location, [protocol]);
        return (servers ?? new List<Server>()).FirstOrDefault();
    }
}