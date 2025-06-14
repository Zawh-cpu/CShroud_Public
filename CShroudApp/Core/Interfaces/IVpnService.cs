using Ardalis.Result;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface IVpnService
{
    Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials);
    Task DisableAsync();
    
    Task RestartAsync(VpnMode mode, VpnConnectionCredentials credentials);
    
    bool IsRunning { get; }
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
}