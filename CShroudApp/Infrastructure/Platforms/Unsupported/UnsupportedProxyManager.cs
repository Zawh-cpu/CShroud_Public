using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Platforms.Unsupported;

public class UnsupportedProxyManager : IProxyManager
{
    public Task EnableAsync(string proxyAddress, List<string> excludedHosts)
    {
        throw new NotSupportedException();
    }

    public Task DisableAsync(string? oldAddress, List<string>? excludedHosts)
    {
        throw new NotSupportedException();
    }

    public ProxyStruct? GetProxyData()
    {
        throw new NotSupportedException();
    }
}