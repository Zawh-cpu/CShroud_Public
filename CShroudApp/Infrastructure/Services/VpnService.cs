using Ardalis.Result;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    public bool IsRunning => _vpnCore.IsRunning;
    
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    
    private readonly IVpnCore _vpnCore;
    
    public VpnProtocol[] SupportedProtocols => _vpnCore.SupportedProtocols;

    public VpnService(IVpnCore vpnCore)
    {
        _vpnCore = vpnCore;

        _vpnCore.CoreEnabled += OnCoreEnabled;
        _vpnCore.CoreDisabled += OnCoreDisabled;
    }
    
    public async Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials)
    {
        if (IsRunning) return Result.Conflict();
        if (!SupportedProtocols.Contains(credentials.Protocol)) return Result.Invalid();
        
        return (await _vpnCore.EnableAsync(mode, credentials)).Map();
    }

    public Task DisableAsync()
    {
        throw new NotImplementedException();
    }

    public Task RestartAsync(VpnMode mode, VpnConnectionCredentials credentials)
    {
        throw new NotImplementedException();
    }

    private void OnCoreEnabled(object? sender, EventArgs e)
    {
        VpnEnabled?.Invoke(this, e);
    }
    
    private void OnCoreDisabled(object? sender, EventArgs e)
    {
        VpnDisabled?.Invoke(this, e);
    }
}