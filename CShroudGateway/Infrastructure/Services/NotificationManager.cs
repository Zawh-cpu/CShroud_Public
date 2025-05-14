using System.Text.Json;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Services;

public class NotificationManager : INotificationManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public NotificationManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public void CallAndForget(List<Mail> notifications)
    {
        _ = _httpClientFactory
            .CreateClient("TelegramHook")
            .PostAsJsonAsync("api/v1/hook/mail", notifications);
    }
}