using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Platforms.Linux.Services;

public class LinuxProxyManager : IProxyManager
{
    public Task EnableAsync(string proxyAddress, List<string> excludedHosts)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(string? oldAddress, List<string>? excludedHosts)
    {
        throw new NotImplementedException();
    }

    public ProxyStruct? GetProxyData()
    {
        throw new NotImplementedException();
    }
}