using System.Text.Json.Serialization;

namespace CShroudDAW.Core.Configs;

public enum DebugMode
{
    None,
    Debug,
    Info,
    Warning,
    Error
}

public enum VpnRuntimeCore
{
    Xray
}

public class ApplicationConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter<DebugMode>))]
    public DebugMode DebugMode { get; set; } = DebugMode.None;
    public VpnSettings Vpn { get; set; } = new VpnSettings();
    
    public string GatewayAddress { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;

    public class VpnSettings
    {
        [JsonConverter(typeof(JsonStringEnumConverter<VpnRuntimeCore>))]
        public VpnRuntimeCore RuntimeCore { get; set; } = VpnRuntimeCore.Xray;
        public VpnCoresValues Cores { get; set; } = new();
        public VpnProtocolAttributes Protocols { get; set; } = new();

        public class VpnCoresValues
        {
            public VpnCoreSetting Xray { get; set; } = new("", "");
            
            public record VpnCoreSetting(string Args, string ApiAddress);  
        }

        public class VpnProtocolAttributes
        {
            public ProtocolAttributes Vless { get; set; } = new();
            public ProtocolAttributes WireGuardOverVless { get; set; } = new();
            
            public class ProtocolAttributes
            {
                public string PrivateKey { get; set; } = String.Empty;
            }
        }
        
    }
}