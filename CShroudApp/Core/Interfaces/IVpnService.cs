using CShroudApp.Core.Entities.Vpn;

namespace CShroudApp.Core.Interfaces;

public interface IVpnService
{
    Task EnableAsync(VpnMode mode);
    Task DisableAsync();
    Task RestartAsync(VpnMode mode);
    
    bool IsRunning { get; }
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
}