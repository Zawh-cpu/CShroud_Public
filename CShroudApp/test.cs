using System.Text.Json.Nodes;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp;

public class test
{
    public async static Task PseudoMain(IServiceProvider serviceProvider)
    {
        var credentials = new VpnConnectionCredentials()
        {
            Host = "localhost",
            Port = 443,
            IpAddressV4 = "127.0.0.1",
            IpAddressV6 = "::1",
            Location = "frankfurt",
            Obtained = DateTime.UtcNow,
            Protocol = VpnProtocol.Vless,
            YourIPv4Address = "127.0.0.1",
            TransparentHosts = new()
            {
                "frankfurt.reality.zawh.ru",
            },
            Credentials = new JsonObject()
            {
                ["Host"] = "frankfurt.reality.zawh.ru",
                ["Port"] = 443,
                ["Uuid"] = "8d50da4e-fff4-4188-bbd1-7d620c7296f0",
                ["Flow"] = "xtls-rprx-vision",
                ["ServerName"] = "google.com",
                ["Insecure"] = "false",
                ["PublicKey"] = "8AZQljbSjvPMPvcjizPM4JpTmcHBPWx_stM_h0gofEI",
                ["ShortId"] = "4ae60b64b5cd"
            }
        };
        
        var vpnService = serviceProvider.GetRequiredService<IVpnService>();

        try
        {
            var res = await vpnService.EnableAsync(VpnMode.Proxy, credentials);

            Console.WriteLine($"RESULT OF RUN-VPN ATTEMPT = {res.Status}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}