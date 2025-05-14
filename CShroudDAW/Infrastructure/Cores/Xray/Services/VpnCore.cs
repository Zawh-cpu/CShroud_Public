using System.Diagnostics;
using CShroudDAW.Application.Factories;
using CShroudDAW.Core.Entities;
using CShroudDAW.Core.Interfaces;
using CShroudDAW.Infrastructure.Data.Config;
using CShroudDAW.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace CShroudDAW.Infrastructure.Cores.Xray.Services;

public class VpnCore : IVpnCore
{
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    public List<VpnProtocol> SupportedProtocols { get; } = new()
    {
        VpnProtocol.Vless
    };
    
    public bool IsProtocolSupported(VpnProtocol protocol) => SupportedProtocols.Contains(protocol);

    private readonly BaseProcess _process;
    
    public VpnCore(IProcessFactory processFactory, IOptions<ApplicationConfig> options)
    {
        string runtimeName;
        switch (PlatformService.GetPlatform())
        {
            case "Windows":
                runtimeName = "xray.exe";
                break;
            case "Linux":
                runtimeName = "xray";
                break;
            default:
                runtimeName = "xray";
                break;
        }
        var processStartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.CurrentDirectory, "Binaries", "Cores", "Xray", PlatformService.GetFullname(), runtimeName),
            Arguments = options.Value.Vpn.Cores.Xray.Args,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        _process = processFactory.Create(processStartInfo, options.Value.DebugMode);
        _process.ProcessExited += OnProcessExited;
        _process.ProcessStarted += OnProcessStarted;
    }
    
    public void Enable()
    {
        if (IsRunning) return;
        _process.Start();
    }

    public void Disable()
    {
        if (!IsRunning) return;
        _process.Kill();
    }

    public async Task DisableAsync()
    {
        if (!IsRunning) return;
        await _process.KillAsync();
    }

    public bool IsRunning => _process.IsRunning;

    private void OnProcessExited(object? sender, EventArgs e)
    {
        ProcessExited?.Invoke(this, e);
    }

    private void OnProcessStarted(object? sender, EventArgs e)
    {
        ProcessStarted?.Invoke(this, e);
    }
}