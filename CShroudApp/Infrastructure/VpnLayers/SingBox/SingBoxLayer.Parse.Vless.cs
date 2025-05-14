using CShroudApp.Core.Entities.Vpn.Bounds;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public partial class SingBoxLayer
{
    private static SingBoxConfig.BoundObject ParseVlessBound(Vless vless)
    {
        return new SingBoxConfig.BoundObject()
        {
            Type = "vless",
            Tag = vless.Tag,
            Extra = new JObject()
            {
                ["server"] = vless.Host,
                ["server_port"] = vless.Port,
                ["uuid"] = vless.Uuid,
                ["flow"] = vless.Flow,
                ["packet_encoding"] = vless.PacketEncoding,
                ["tls"] = new JObject()
                {
                    ["enabled"] = true,
                    ["server_name"] = vless.ServerName,
                    ["insecure"] = vless.Insecure,
                    ["utls"] = new JObject()
                    {
                        ["enabled"] = true,
                        ["fingerprint"] = vless.Fingerprint,
                    },
                    ["reality"] = new JObject()
                    {
                        ["enabled"] = true,
                        ["public_key"] = vless.PublicKey,
                        ["short_id"] = vless.ShortId
                    }
                }
            }
        };
    }
}