using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Ardalis.Result;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.Utils;
using CShroudApp.Infrastructure.Processes;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;
using CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;
using CShroudApp.Infrastructure.VpnCores.SingBox.Mappers;

namespace CShroudApp.Infrastructure.VpnCores.SingBox;

public class SingBoxCore : IVpnCore
{
    public event EventHandler? CoreEnabled;
    public event EventHandler? CoreDisabled;
    public bool IsRunning => _process.IsRunning;

    private static readonly string PathToCoreDirectory = Path.Combine(AppConstants.BinariesDirectory, "Cores", "SingBox");
    
    public VpnProtocol[] SupportedProtocols { get; } = [VpnProtocol.Http, VpnProtocol.Socks, VpnProtocol.Tun, VpnProtocol.Vless];

    private readonly Dictionary<VpnProtocol, Func<JsonObject, BoundObject?>> _boundMappers = new()
    {
        [VpnProtocol.Vless] = VlessMapper.Map
    };
    
    private TopConfig _config = new();
    
    private readonly ApplicationConfig _settings;
    private readonly IInternalDataManager _internalDataManager;
    
    private BaseProcess _process;

    public SingBoxCore(ApplicationConfig settings, IInternalDataManager internalDataManager, ProcessFactory processFactory)
    {
        _settings = settings;
        _internalDataManager = internalDataManager;
        
        string runtimeName;
        switch (PlatformInformation.GetPlatform())
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
            FileName = Path.Combine(PathToCoreDirectory, PlatformInformation.GetFullname(), runtimeName),
            Arguments = "run -c stdin",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        _process = processFactory.Create(processStartInfo, _settings.LogLevel);
    }

    private void SetupLogs()
    {
        var levels = new Dictionary<LogLevelMode, string>
        {
            [LogLevelMode.Off] = "none",
            [LogLevelMode.Info] = "info",
            [LogLevelMode.Warning] = "warn",
            [LogLevelMode.Error] = "error",
            [LogLevelMode.Debug] = "debug"
        };
        
        _config.Log.Level = levels.GetValueOrDefault(_settings.LogLevel, "none");
    }

    private void SetupDns(List<string> transparedHosts)
    {
        _config.Dns.Servers.AddRange(
            new Dictionary<string, object>
            {
                ["Tag"] = "remote",
                ["Address"] = _settings.Vpn.PreferredProxy.Contains("://") ? _settings.Vpn.PreferredProxy : $"tcp://{_settings.Vpn.PreferredProxy}",
                ["Strategy"] = "prefer_ipv4",
                ["Detour"] = "proxy"
            },
            new Dictionary<string, object>
            {
                ["Tag"] = "local",
                ["Address"] = "223.5.5.5",
                ["Strategy"] = "prefer_ipv4",
                ["Detour"] = "direct"
            },
            new Dictionary<string, object>
            {
                ["Tag"] = "block",
                ["Address"] = "rcode://success"
            },
            new Dictionary<string, object>
            {
                ["Tag"] = "local_local",
                ["Address"] = "223.5.5.5",
                ["Detour"] = "direct"
            }
        );
        
        _config.Dns.Rules.AddRange(
            new DnsObject.Rule()
            {
                Server = "local_local",
                Domain = transparedHosts,
            },
            new DnsObject.Rule()
            {
                Server = "local",
                RuleSet = new List<string> { "geosite-cn" }
            });
        
        // CHANGE IF SPLIT TUNNELING OR WHITE_LIST
        _config.Dns.Final = "remote";
    }

    private void SetupInbounds()
    {
        _config.Inbounds.AddRange(
            new SocksBound
            {
                Tag = "socks",
                Listen = _settings.Vpn.Inputs.Socks.Host,
                ListenPort = _settings.Vpn.Inputs.Socks.Port,
                Sniff = true,
                SniffOverrideDestination = true
            },
            
            new HttpBound()
            {
                Tag = "http",
                Listen = _settings.Vpn.Inputs.Http.Host,
                ListenPort = _settings.Vpn.Inputs.Http.Port,
                Sniff = true,
                SniffOverrideDestination = true
            });
    }

    private void SetupOutbounds(VpnConnectionCredentials credentials)
    {
        if (!_boundMappers.TryGetValue(credentials.Protocol, out var mapper))
            throw new NotSupportedException();

        var mapped = mapper(credentials.Credentials);
        if (mapped == null) throw new NotSupportedException();
        
        _config.Outbounds.AddRange(mapped,
            new BoundObject()
            {
                Type = "direct",
                Tag = "direct"
            },
            new BoundObject()
            {
                Type = "block",
                Tag = "block"
            },
            new BoundObject()
            {
                Type = "dns",
                Tag = "dns_out"
            }
        );
    }

    private void SetupRoutes()
    {
        /*_config.Route.Rules.AddRange(
            new RouteObject.Rule()
            {
                Outbound = "dns_out",
                Protocol = [ "dns" ]
            },
            new RouteObject.Rule()
            {
                Outbound = "proxy",
                Domain = [ "googleapis.cn", "gstatic.com" ],
                DomainSuffix = [ ".googleapis.cn", ".gstatic.com" ]
            },
            new RouteObject.Rule()
            {
                Outbound = "block",
                Network = [ "udp" ],
                Port = [ 443 ]
            },
            new RouteObject.Rule()
            {
                Outbound = "direct",
                RuleSet = [ "geosite-private" ]
            },
            new RouteObject.Rule()
            {
                Outbound = "direct",
                IpCidr = _internalDataManager.InternalDirectIPs.ToArray()
            },
            new RouteObject.Rule()
            {
                Outbound = "direct",
                Domain = _internalDataManager.InternalDirectDomains.ToArray(),
                DomainSuffix = _internalDataManager.InternalDirectDomains.Select(x => "." + x).ToArray()
            },
            new RouteObject.Rule()
            {
                Outbound = "direct",
                RuleSet = [ "geoip-cn" ]
            },
            new RouteObject.Rule()
            {
                Outbound = "direct",
                RuleSet = [ "geosite-cn" ]
            }
            );*/
        
        _config.Route.RuleSet.AddRange(
            new RouteObject.RuleSetObject()
            {
                Tag = "geosite-private",
                Type = "local",
                Format = "binary",
                Path = Path.Combine(AppConstants.InternalGeoRulesPath, "geosite-private.srs")
            },
            new RouteObject.RuleSetObject()
            {
                Tag = "geosite-cn",
                Type = "local",
                Format = "binary",
                Path = Path.Combine(AppConstants.InternalGeoRulesPath, "geosite-cn.srs")
            },
            new RouteObject.RuleSetObject()
            {
                Tag = "geoip-cn",
                Type = "local",
                Format = "binary",
                Path = Path.Combine(AppConstants.InternalGeoRulesPath, "geoip-cn.srs")
            });
    }

    private void SetupExperimental()
    {
        _config.Experimental["CacheFile"] = new
        {
            Enabled = true,
            Path = Path.Combine(PathToCoreDirectory, "cache.db")
        };
    }
    
    public async Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials)
    {
        Console.WriteLine("TRYING ENABLE IT");
        try
        {
            _config = new TopConfig();
            SetupLogs();
            SetupDns(credentials.TransparentHosts);
            SetupInbounds();
            SetupOutbounds(credentials);
            SetupRoutes();
            //SetupExperimental();

            _process.Start(reactivateProcess: true);
            var config = JsonSerializer.Serialize(_config, SingBoxJsonContext.Default.TopConfig);
            Console.WriteLine(config);

            await _process.StandardInput.WriteAsync(config);
        }
        catch (Exception e)
        {
            Console.WriteLine($"EXCEPTION: {e}");
        }
        
        
        return Result.Success();
    }
}