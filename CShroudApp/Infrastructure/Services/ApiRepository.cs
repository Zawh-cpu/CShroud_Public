using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ardalis.Result;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.JsonContexts;

namespace CShroudApp.Infrastructure.Services;

public class ApiRepository : IApiRepository
{
    public DateTime LastInternetInterrupt = DateTime.MinValue;
    
    private bool CanMakeRequest => LastInternetInterrupt - DateTime.UtcNow < TimeSpan.FromSeconds(45);
    private HttpClient _client;
    private INotificationManager _notificationManager;
    
    private Token? _refreshToken;
    private Token? _actionToken;
    
    public string? RefreshToken
    {
        get => _refreshToken?.Data;
        set => _refreshToken = Token.Parse(value!);
    }
    
    public string? ActionToken
    {
        get
        {
            if (_refreshToken is null) return null;
            if (_actionToken is null || _actionToken?.Expiration - DateTime.UtcNow <= TimeSpan.FromSeconds(10) )
            {
                var token = Task.Run(async() => await RefreshActionTokenAsync(RefreshToken!)).Result;
                if (!token.IsSuccess)
                    return null;
                _actionToken = Token.Parse(token.Value.ActionToken);
            }
            
            return _actionToken?.Data;
        }
        set
        {
            if (value is not null)
                _actionToken = Token.Parse(value);
        }
    }

    public ApiRepository(IHttpClientFactory httpClientFactory, INotificationManager notificationManager)
    {
        _client = httpClientFactory.CreateClient("CrimsonShroudApiHook");
        _notificationManager = notificationManager;
    }
    
    private void OnInternetConnectionLoss()
    {
        LastInternetInterrupt = DateTime.UtcNow;
        
        _notificationManager.OnInternetInterrupt();
    }
    
    private async Task<Result<HttpResponseMessage>> MakeRequestAsync(HttpRequestMessage request)
    {
        if (!CanMakeRequest) return Result.Unavailable();
        
        HttpResponseMessage response = null!;
        
        try
        {
            response = await _client.SendAsync(request);
            if (LastInternetInterrupt != DateTime.MinValue)
            {
                LastInternetInterrupt = DateTime.MinValue;
                _notificationManager.OnInternetConnectionRestored();
            }
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == null)
            {
                OnInternetConnectionLoss();
                return Result.Unavailable();
            }
        }

        if (!response.IsSuccessStatusCode)
        {   
            return Result.Error();
        }
        
        return response;
    }

    public async Task<Result<SignInDto>> SignInAsync(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/signin");
        request.Content = new StringContent(JsonSerializer.Serialize<SignInDataDto>(new SignInDataDto { Login = username, Password = password}, DtoJsonContext.Default.SignInDataDto), Encoding.UTF8, "application/json");
        
        var response = await MakeRequestAsync(request);
        if (!response.IsSuccess) return response.Map();
        
        var stream = await response.Value.Content.ReadAsStreamAsync();
        
        var dto = await JsonSerializer.DeserializeAsync(stream, DtoJsonContext.Default.SignInDto);
        if (dto is null) return Result.Error();

        return dto;
    }
    
    public async Task<Result<QuickAuthSessionDto>> BeginQuickAuthSessionAsync()
    {
        var response = await MakeRequestAsync(new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/quick-auth"));
        if (!response.IsSuccess) return response.Map();
        
        var stream = await response.Value.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(stream, DtoJsonContext.Default.QuickAuthSessionDto);
        if (dto is null) return Result.Error();

        return dto;
    }
    
    public async Task<Result<SignInDto>> FinalizeQuickAuthSessionAsync(QuickAuthDto data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/auth/quick-auth/{data.SessionId}/finalize");
        request.Content = new StringContent(data.SecretLoginCode, Encoding.UTF8);
        
        var response = await MakeRequestAsync(request);
        if (!response.IsSuccess) return response.Map();
        
        var stream = await response.Value.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(stream, DtoJsonContext.Default.SignInDto);
        if (dto is null) return Result.Error();

        return dto;
    }
    
    public async Task<Result<ActionTokenRefreshDto>> RefreshActionTokenAsync(string refreshToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/auth/refresh");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);
        
        var response = await MakeRequestAsync(request);
        if (!response.IsSuccess) return response.Map();
        
        var stream = await response.Value.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(stream, DtoJsonContext.Default.ActionTokenRefreshDto);
        if (dto is null) return Result.Error();

        return dto;
    }
    
    public async Task<Result<GetUserDto>> GetUserInformationAsync()
    {
        var token = ActionToken;
        if (token is null) return Result.Unavailable();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/user/me");
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await MakeRequestAsync(request);
        if (!response.IsSuccess) return response.Map();
        
        var stream = await response.Value.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync(stream, DtoJsonContext.Default.GetUserDto);
        if (dto is null) return Result.Error();

        return dto;
    }
    
    public async Task Test()
    {
        var url = "https://www.google.com";

        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Success! Status code: {(int)response.StatusCode}");
            }
            else
            {
                Console.WriteLine($"Failed. Status code: {(int)response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            //Console.WriteLine(ex);HttpResponseMessage
            Console.WriteLine(ex.StatusCode == null);
            Console.WriteLine(ex.Message);
            Console.WriteLine("Network error or no internet connection.");
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Request timed out (possible no internet).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}