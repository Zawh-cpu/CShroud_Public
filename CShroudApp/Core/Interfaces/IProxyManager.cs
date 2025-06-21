using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface IProxyManager
{
    ProxyData? CachedProxy { get; set; }
    ProxyData? GetProxyData();
    
    void SetupProxy(ProxyData proxy, bool savePreviousProxy = true);
    void DisableProxy();
}