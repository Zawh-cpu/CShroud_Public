using System.Text;
using CShroud.Core.Domain.Entities;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class TelegramManager : ITelegramManager
{
    private readonly HttpClient _httpClient = new HttpClient();
    private BaseConfig _baseConfig;
    
    public TelegramManager(BaseConfig baseConfig)
    {
        _baseConfig = baseConfig;
        _httpClient.BaseAddress = new Uri(_baseConfig.TelegramLink);
    }
    
    public void RateNotification(string jsonContent)
    {
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        _ = _httpClient.PostAsync($"{_baseConfig.TelegramLink}/api/rateNotifications", content);
    }
}