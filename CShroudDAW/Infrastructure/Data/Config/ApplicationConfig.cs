namespace CShroudDAW.Infrastructure.Data.Config;

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
    public DebugMode DebugMode { get; set; } = DebugMode.None;
    public VpnSettings Vpn { get; set; } = new VpnSettings();

    public class VpnSettings
    {
        public VpnRuntimeCore RuntimeCode { get; set; } = VpnRuntimeCore.Xray;
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