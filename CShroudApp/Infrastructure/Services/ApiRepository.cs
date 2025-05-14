using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.Services;

public class ApiRepository : IApiRepository
{
    async public Task<VpnNetworkCredentials> ConnectToVpnNetworkAsync(List<VpnProtocol> supportedProtocols, string location)
    {
        return new VpnNetworkCredentials()
        {
            ServerHost = "localhost",
            ServerPort = 443,
            IPv4 = "127.0.0.1",
            IPv6 = "::1",
            Location = location,
            Obtained = DateTime.UtcNow,
            Protocol = VpnProtocol.Vless,
            YourIPv4Address = "127.0.0.1",
            TransparentHosts = new()
            {
                "frankfurt.reality.zawh.ru",
            },
            Credentials = new JObject()
            {
                ["Host"] = "frankfurt.reality.zawh.ru",
                ["Port"] = "443",
                ["Uuid"] = "8d50da4e-fff4-4188-bbd1-7d620c7296f0",
                ["Flow"] = "xtls-rprx-vision",
                ["ServerName"] = "google.com",
                ["Insecure"] = "false",
                ["PublicKey"] = "8AZQljbSjvPMPvcjizPM4JpTmcHBPWx_stM_h0gofEI",
                ["ShortId"] = "4ae60b64b5cd"
            }
        };
    }
}