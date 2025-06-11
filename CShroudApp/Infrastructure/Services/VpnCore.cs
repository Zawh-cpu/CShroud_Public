using System.Diagnostics;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class VpnCore : IVpnCore
{
    private readonly IVpnCoreLayer _vpnCoreLayer;

    public bool IsRunning => _vpnCoreLayer.IsRunning;
    
    public bool IsSupportProtocol(VpnProtocol protocol) => _vpnCoreLayer.IsProtocolSupported(protocol);
    public List<VpnProtocol> SupportedProtocols => _vpnCoreLayer.SupportedProtocols;

    public VpnCore(IVpnCoreLayer vpnCoreLayer)
    {
        _vpnCoreLayer = vpnCoreLayer;
        _vpnCoreLayer.ProcessStarted += OnProcessStarted;
        _vpnCoreLayer.ProcessExited += OnProcessStopped;
    }

    public void ApplyConfiguration(SettingsConfig settings)
    {
        _vpnCoreLayer.ConcatConfigs(settings);
    }

    public void ApplySplitTunneling(SettingsConfig.SplitTunnelingObject splitTunnelingObject)
    {
        _vpnCoreLayer.ApplySplitTunneling(splitTunnelingObject);
    }
    
    public async Task EnableAsync()
    {
        if (!IsRunning)
        {
            await _vpnCoreLayer.StartProcessAsync();
        }
    }

    public async Task DisableAsync()
    {
        if (IsRunning)
        {
            await _vpnCoreLayer.KillProcessAsync();
            Console.WriteLine("PROCESS KILLED");
            
        }
    }

    public void FixDnsIssues(List<string> transparentHosts)
    {
        _vpnCoreLayer.FixDnsIssues(transparentHosts);
    }
    
    public void ClearMainInbounds()
    {
        _vpnCoreLayer.RemoveInbound("main-net-", true);
    }

    public void AddInbound(IVpnBound bound)
    {
        _vpnCoreLayer.AddInbound(bound);
    }
    
    public void ChangeMainOutbound(IVpnBound bound)
    {
        _vpnCoreLayer.RemoveOutbound("main-net-", true);
        bound.Tag = "main-net-outbound";
        _vpnCoreLayer.AddOutbound(bound, 0);
    }

    private void OnProcessStarted(object? sender, EventArgs e)
    {
        VpnEnabled?.Invoke(this, e);
    }

    private void OnProcessStopped(object? sender, EventArgs e)
    {
        VpnDisabled?.Invoke(this, e);
    }

    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
}