using System.Text.Json.Serialization;
using CShroudApp.Core.Entities.Vpn;
using Microsoft.Extensions.Configuration;

namespace CShroudApp.Infrastructure.Data.Config;

public enum DebugMode
{
    None,
    Debug,
    Info,
    Warning,
    Error
}

public enum VpnCore
{
    SingBox,
    Xray
}

public class SettingsConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DebugMode DebugMode { get; set; } = DebugMode.None;
    public NetworkObject Network { get; set; } = new();
    public SplitTunnelingObject SplitTunneling { get; set; } = new();

    public class NetworkObject
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VpnMode Mode { get; set; } = VpnMode.ProxyAndTun;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VpnCore Core { get; set; } = VpnCore.SingBox;
        public ProxyObject Proxy { get; set; } = new();

        public class ProxyObject
        {
            public List<string> ExcludedHosts { get; set; } = new();
            public VpnProtocol Default { get; set; } = VpnProtocol.Socks;
            public ProxyData Http { get; set; } = new() { Host = "127.0.0.1", Port = 11809 };
            public ProxyData Socks { get; set; } = new() { Host = "127.0.0.1", Port = 11808 };

            public class ProxyData
            {
                public string Host { get; set; } = "127.0.0.1";
                public uint Port { get; set; } = 1000;
            }
        }
    }

    public class SplitTunnelingObject
    {
        public bool Enabled { get; set; } = true;
        public bool WhiteList { get; set; } = false;
        public List<string> Applications { get; set; } = new();
        public List<string> Paths { get; set; } = new();
        public List<string> Domains { get; set; } = new();
    }


    public static SettingsConfig MakeDefault()
    {
        return new SettingsConfig()
        {
            DebugMode = DebugMode.None,
            Network = new NetworkObject(),
            SplitTunneling = new SplitTunnelingObject()
        };
    }
}