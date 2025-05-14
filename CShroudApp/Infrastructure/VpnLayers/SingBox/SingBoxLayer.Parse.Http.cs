using CShroudApp.Core.Entities.Vpn.Bounds;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public partial class SingBoxLayer
{
    private static SingBoxConfig.BoundObject ParseHttpBound(Http data)
    {
        return new SingBoxConfig.BoundObject()
        {
            Type = "http",
            Tag = data.Tag,
            Extra = new JObject()
            {
                ["listen"] = data.Host,
                ["listen_port"] = data.Port,
                ["sniff"] = data.Sniff,
                ["sniff_override_destination"] = data.SniffOverrideDestination
            }
        };
    }
}