using System.Diagnostics;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Data.Json.Policies;
using CShroudApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public partial class SingBoxLayer : IVpnCoreLayer
{
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    public List<VpnProtocol> SupportedProtocols { get; } = new()
    {
        VpnProtocol.Vless, VpnProtocol.Http, VpnProtocol.Socks, VpnProtocol.Tun
    };
    
    public bool IsProtocolSupported(VpnProtocol protocol) => SupportedProtocols.Contains(protocol);

    private BaseProcess _process;
    
    private SingBoxConfig _configuration = new();

    private readonly IProcessFactory _processFactory;
    private readonly SettingsConfig _settings;
    
    private readonly Dictionary<VpnProtocol, Func<IVpnBound, SingBoxConfig.BoundObject>> _vpnProtocolsHandlers = new()
    {
        { VpnProtocol.Vless, inbound => ParseVlessBound((Vless)inbound) },
        { VpnProtocol.Http, inbound => ParseHttpBound((Http)inbound) },
        { VpnProtocol.Socks, inbound => ParseSocksBound((Socks)inbound) },
        { VpnProtocol.Tun, inbound => ParseTunBound((Tun)inbound) },
    };
    
    public SingBoxLayer(IProcessFactory processFactory, IOptions<SettingsConfig> settings)
    {
        _processFactory = processFactory;
        _settings = settings.Value;

        _process = MakeStartupProcess(_processFactory, _settings);
    }

    private BaseProcess MakeStartupProcess(IProcessFactory processFactory, SettingsConfig settings)
    {
        string runtimeName;
        switch (PlatformService.GetPlatform())
        {
            case "Windows":
                runtimeName = "sing-box.exe";
                break;
            case "Linux":
                runtimeName = "sing-box";
                break;
            default:
                runtimeName = "sing-box";
                break;
        }
        var processStartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.CurrentDirectory, "Binaries", "Cores", "SingBox", PlatformService.GetFullname(), runtimeName),
            Arguments = "run -c stdin",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        var process = processFactory.Create(processStartInfo, settings.DebugMode);
        process.ProcessExited += OnProcessExited;
        process.ProcessStarted += OnProcessStarted;

        return process;
    }
    
    public void AddInbound(IVpnBound bound, int index = Int32.MaxValue)
    {
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        if (index < 0 || index > _configuration.Inbounds.Count) _configuration.Inbounds.Add(action(bound));
        else _configuration.Inbounds.Insert(index, action(bound));
    }

    public void AddOutbound(IVpnBound bound, int index = Int32.MaxValue)
    {
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        if (index < 0 || index > _configuration.Outbounds.Count) _configuration.Outbounds.Add(action(bound));
        else _configuration.Outbounds.Insert(index, action(bound));
    }

    public void RemoveInbound(string tag, bool startsWithMode = false)
    {
        if (startsWithMode) _configuration.Inbounds.RemoveAll(x => x.Tag.StartsWith(tag));
        else _configuration.Inbounds.RemoveAll(x => x.Tag == tag);
    }

    public void RemoveOutbound(string tag, bool startsWithMode = false)
    {
        if (startsWithMode) _configuration.Outbounds.RemoveAll(x => x.Tag.StartsWith(tag));
        else _configuration.Outbounds.RemoveAll(x => x.Tag == tag);
    }

    public async Task StartProcessAsync()
    {
        if (IsRunning) return;
        if (_process.HasExited) _process = MakeStartupProcess(_processFactory, _settings);

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var x = JsonConvert.SerializeObject(_configuration, settings);
        
        Console.WriteLine(x);
        
        _process.Start();
        await _process.StandardInput.WriteAsync(x);
        _process.StandardInput.Close();
    }

    public void ConcatConfigs(SettingsConfig settings)
    {
        _configuration.Log.Disabled = settings.DebugMode == DebugMode.None ? true : false;
        _configuration.Log.Level = settings.DebugMode.ToString().ToLowerInvariant();
        
        _configuration.Dns.Servers.Add(new JObject()
        {
            ["tag"] = "remote",
            ["address"] = "8.8.8.8",
            ["detour"] = "main-net-outbound",
            ["strategy"] = "ipv4_only"
        });
        
        _configuration.Dns.Servers.Add(new JObject()
        {
            ["tag"] = "block",
            ["address"] = "rcode://success"
        });
        
        _configuration.Dns.Servers.Add(new JObject()
        {
            ["tag"] = "local",
            ["address"] = "8.8.8.8",
            ["detour"] = "direct"
        });

        _configuration.Dns.Final = "remote";
        
        _configuration.Outbounds.Add(new SingBoxConfig.BoundObject()
        {
            Type = "direct",
            Tag = "direct"
        });
        
        _configuration.Outbounds.Add(new SingBoxConfig.BoundObject()
        {
            Type = "block",
            Tag = "block"
        });
        
        _configuration.Outbounds.Add(new SingBoxConfig.BoundObject()
        {
            Type = "dns",
            Tag = "dns_out"
        });
    }
    
    public async Task KillProcessAsync()
    {
        if (!IsRunning) return;
        _configuration = new SingBoxConfig();
        await _process.KillAsync();
    }

    public void FixDnsIssues(List<string> transparentHosts)
    {
        _configuration.Route.AutoDetectInterface = true;
        
        _configuration.Dns.Rules.Add(new JObject()
        {
            ["server"] = "local",
            ["domain"] = JArray.FromObject(transparentHosts)
        });

        _configuration.Route.Rules.Add(new JObject()
        {
            ["outbound"] = "dns_out",
            ["protocol"] = new JArray() { "dns" }
        });


        _configuration.Route.Rules.Add(new JObject()
        {
            ["outbound"] = "direct",
            ["process_name"] = new JArray() { "wv2ray.exe",
                //"CShroudApp.exe",
                "v2ray.exe",
                "SagerNet.exe",
                "v2ray.exe",
                "v2ray.exe",
                "xray.exe",
                "wxray.exe",
                "tuic-client.exe",
                "tuic.exe",
                "sing-box-client.exe",
                "sing-box.exe" }
        });

        _configuration.Route.Rules.Add(new JObject()
        {
            ["outbound"] = "dns_out",
            ["port"] = 53,
            ["process_name"] = new JArray() { "wv2ray.exe",
                //"CShroudApp.exe",
                "v2ray.exe",
                "SagerNet.exe",
                "v2ray.exe",
                "v2ray.exe",
                "xray.exe",
                "wxray.exe",
                "tuic-client.exe",
                "tuic.exe",
                "sing-box-client.exe",
                "sing-box.exe" }
        });
        
        _configuration.Route.Final = "main-net-outbound";
    }

    public void ApplySplitTunneling(SettingsConfig.SplitTunnelingObject splitTunnelingObject)
    {
        if (splitTunnelingObject.Applications.Any())
            _configuration.Route.Rules.Add(new JObject()
            {
                ["outbound"] = splitTunnelingObject.WhiteList is true ? "direct" : "main-net-outbound",
                ["process_name"] = JArray.FromObject(splitTunnelingObject.Applications)
            });
        
        if (splitTunnelingObject.Domains.Any())
            _configuration.Route.Rules.Add(new JObject()
            {
                ["outbound"] = splitTunnelingObject.WhiteList is true ? "direct" : "main-net-outbound",
                ["domain"] = JArray.FromObject(splitTunnelingObject.Domains)
            });
        
        if (splitTunnelingObject.Paths.Any())
            _configuration.Route.Rules.Add(new JObject()
            {
                ["outbound"] = splitTunnelingObject.WhiteList is true ? "direct" : "main-net-outbound",
                ["process_path"] = JArray.FromObject(splitTunnelingObject.Domains)
            });
        
        /*_configuration.Route.Rules.Insert(1, new JObject()
        {
            ["type"] = "logical",
            ["mode"] = "or",
            ["rules"] = new JArray()
            {
                new JObject()
                {
                    ["process_name"] = JArray.FromObject(splitTunnelingObject.Applications),
                    ["outbound"] = "direct",
                }, 
                new JObject()
                {
                    ["domain"] = JArray.FromObject(splitTunnelingObject.Domains),
                    ["outbound"] = "direct",
                }, 
            },
            ["action"] = "route",
            ["outbound"] = "direct",
        });*/
        
        if (!splitTunnelingObject.WhiteList)
            _configuration.Route.Final = "direct";
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