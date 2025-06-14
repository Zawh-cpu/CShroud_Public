using System.Text.Json;
using System.Text.Json.Nodes;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;
using CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.Mappers;

public record VlessCredentials(
    string Host,
    uint Port,
    string Uuid,
    string Flow,
    string ServerName,
    string Insecure,
    string PublicKey,
    string ShortId);

public static class VlessMapper
{
    public static VlessBound? Map(JsonObject credentials)
    {
        // var creds = (VlessCredentials)credentials;
        var creds = credentials.Deserialize<VlessCredentials>(SingBoxJsonContext.Default.VlessCredentials);
        if (creds == null) return null;

        return new VlessBound()
        {
            Server = creds.Host,
            ServerPort = creds.Port,
            Uuid = creds.Uuid,
            Flow = creds.Flow,
            PacketEncoding = "xudp",
            Tls = new VlessBound.TlsObject()
            {
                Enabled = true,
                ServerName = creds.ServerName,
                Insecure = false,
                Utls = new VlessBound.TlsObject.UtlsObject()
                {
                    Enabled = true,
                    Fingerprint = "random"
                },
                Reality = new VlessBound.TlsObject.RealityObject()
                {
                    Enabled = true,
                    PublicKey = creds.PublicKey,
                    ShortId = creds.ShortId
                }
            }
        };
    }
}