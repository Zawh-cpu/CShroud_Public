using CShroudApp.Core.Entities.Vpn.Bounds;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public partial class SingBoxLayer
{
    private static SingBoxConfig.BoundObject ParseTunBound(Tun data)
    {
        return new SingBoxConfig.BoundObject()
        {
            Type = "tun",
            Tag = data.Tag,
            Extra = new JObject()
            {
                ["interface_name"] = data.InterfaceName,
                ["mtu"] = data.Mtu,
                ["address"] = data.Address is not null ? JArray.FromObject(data.Address) : null,
                ["auto_route"] = data.AutoRoute,
                ["strict_route"] = data.StrictRoute,
                ["stack"] = data.Stack,
                ["sniff"] = data.Sniff
            }
        };
    }
}