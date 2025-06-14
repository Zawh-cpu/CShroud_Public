using Ardalis.Result;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCore
{
    public event EventHandler? CoreEnabled;
    public event EventHandler? CoreDisabled;
    
    bool IsRunning { get; }
    VpnProtocol[] SupportedProtocols { get; }
    
    
    Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials);
}