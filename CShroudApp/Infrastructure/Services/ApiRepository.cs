using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Ardalis.Result;
using CShroudApp.Application.DTOs;
using CShroudApp.Application.Serialization;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Entities.User;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.Shared;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.Services;

public class ApiRepository : IApiRepository
{
    private readonly HttpClient _httpClient;
    private readonly IEventManager _eventManager;

    private Token? _refreshToken;
    private Token? _accessToken;

    public string? ActionToken
    {
        get
        {
            if (_accessToken is null || _accessToken?.Expiration <= DateTime.Now)
            {
                var token = Task.Run(RefreshActionTokenAsync).Result;
                if (!token.IsSuccess)
                    return null;
                _accessToken = Token.Parse(token.Value.ActionToken);
            }
            
            return _accessToken?.Data;
        }
    }

    public string? RefreshToken
    {
        get => _refreshToken?.Data;
        set => _refreshToken = Token.Parse(value!);
    }

    public ApiRepository(IHttpClientFactory httpClientFactory, IEventManager eventManager)
    {
        _httpClient = httpClientFactory.CreateClient("CrimsonShroudApiHook");
        _eventManager = eventManager;
    }
    
    private bool RequireToken()
    {
        var token = ActionToken;
        if (token is null)
        {
            _eventManager.CallAuthSessionExpired();
            return false;
        }
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return true;
    }

    public async Task<Result<ActionRefreshDto>> RefreshActionTokenAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/auth/refresh");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", RefreshToken);
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return Result.Unauthorized();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(
            stream, 
            AppJsonContext.Default.ActionRefreshDto);
        
        if (dto is null) return Result.CriticalError();
        
        return dto;
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

    public async Task<Result<SignInDto>> FinalizeQuickAuthAttemptAsync(QuickAuthDto data)
    {
        Console.WriteLine(data.SessionId);
        Console.WriteLine(data.SecretLoginCode);
        var response = await _httpClient.PostAsync($"/api/v1/auth/quick-auth/{data.SessionId}/finalize", new StringContent(data.SecretLoginCode, Encoding.UTF8));
        Console.WriteLine(response.StatusCode);
        if (!response.IsSuccessStatusCode) return Result.Forbidden();
        
        if (response.StatusCode != HttpStatusCode.OK) return Result.Invalid();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(
            stream, 
            AppJsonContext.Default.SignInDto);

        if (dto is null) return Result.CriticalError();
        
        return dto;
    }

    public async Task<Result<QuickAuthSessionDto>> CreateQuickAuthSessionAsync()
    {
        var response = await _httpClient.PostAsync($"/api/v1/auth/quick-auth", null);
        if (!response.IsSuccessStatusCode) return Result.Forbidden();
        
        if (response.StatusCode != HttpStatusCode.OK) return Result.Invalid();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(
            stream, 
            AppJsonContext.Default.QuickAuthSessionDto);

        if (dto is null) return Result.CriticalError();
        
        return dto;
    }

    public async Task<Result<User>> GetUserInformationAsync()
    {
        Console.WriteLine("GetUserInformationAsync");
        if (!RequireToken()) return Result.Unauthorized();
        Console.WriteLine("GETUSERINFORMATION");
        
        var response = await _httpClient.GetAsync($"/api/v1/user/me");
        
        if (response.StatusCode != HttpStatusCode.OK) return Result.Invalid();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(
            stream, 
            AppJsonContext.Default.GetUserDto);

        if (dto is null) return Result.CriticalError();
        
        return new User()
        {
            Id = dto.Id,
            IsVerified = dto.IsVerified,
            Nickname = dto.Nickname,
            Rate = dto.Rate,
            Role = dto.Role
        };
    }
}