using System.Diagnostics;
using CShroud.Core.Domain.Entities;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class VpnCore: IVpnCore
{
    private readonly BaseProcess _process;
    private IProcessManager _processManager;
    private VpnCoreConfig _vpnConfig;
    public bool IsRunning => _process.IsRunning;
    public event EventHandler VpnStopped = delegate { };
    public event EventHandler VpnStarted = delegate { };
    
    public VpnCore(VpnCoreConfig vpnConfig, IProcessManager processManager)
    {
        _vpnConfig = vpnConfig;
        _processManager = processManager;

        var processStartInfo = new ProcessStartInfo
        {
            FileName = _vpnConfig.Path,
            Arguments = _vpnConfig.Arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        _process = new BaseProcess(processStartInfo, debug: _vpnConfig.Debug);
        
        _process.ProcessStarted += OnProcessStarted;
        _process.ProcessExited += OnProcessStopped;
        
        _processManager.Register(_process);
    }

    public void Start()
    {
        _process.Start();
    }
    
    private void OnProcessStarted(object? sender, EventArgs e)
    {
        VpnStarted?.Invoke(this, e);
    }

    private void OnProcessStopped(object? sender, EventArgs e)
    {
        VpnStopped?.Invoke(this, e);
    }
}