namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;

public class VlessBound : BoundObject
{
    public new string Type { get; set; }= "vless";

    public required string Server { get; set; }
    public required uint ServerPort { get; set; }
    public required string Uuid { get; set; }
    public required string Flow { get; set; }
    public required string PacketEncoding { get; set; }
    public TlsObject? Tls { get; set; }


    public class TlsObject
    {
        public bool Enabled { get; set; } = true;
        public required string ServerName { get; set; }
        public bool Insecure { get; set; } = false;
        public UtlsObject? Utls { get; set; }
        public RealityObject? Reality { get; set; }
        
        public class UtlsObject
        {
            public bool Enabled { get; set; } = true;
            public string Fingerprint { get; set; } = "random";
        }
        
        public class RealityObject
        {
            public bool Enabled { get; set; } = true;
            public required string PublicKey { get; set; }
            public required string ShortId { get; set; }
        }
    }
}