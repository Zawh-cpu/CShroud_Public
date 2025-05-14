using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface IProxyManager
{
    Task EnableAsync(string proxyAddress, List<string> excludedHosts);
    Task DisableAsync(string? oldAddress, List<string>? excludedHosts);
    ProxyStruct? GetProxyData();
}