using System.Net.Http.Json;
using System.Text.Json;
using Ardalis.Result;
using CShroudApp.Application.DTOs;
using CShroudApp.Application.Serialization;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.Services;

public class ApiRepository : IApiRepository
{
    private readonly HttpClient _httpClient;

    public ApiRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CrimsonShroudApiHook");
    }
    
    async public Task<VpnNetworkCredentials?> ConnectToVpnNetworkAsync(List<VpnProtocol> supportedProtocols, string location)
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

    public async Task<Result<FastLoginDto>> TryFastLoginAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/auth/fast_login");
        Console.WriteLine(response.StatusCode);
        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(
            stream, 
            AppJsonContext.Default.FastLoginDto);
        
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }
}