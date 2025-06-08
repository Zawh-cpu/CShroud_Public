using System.Text.Json;
using CShroudGateway.Application.DTOs;
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
        var client = _httpClientFactory
            .CreateClient("TelegramHook");
        
        var a = client.PostAsJsonAsync("api/v1/hook/mail", notifications.Select(x => new MailDto()
        {
            Id = x.Id,
            Recipient = new UserDto()
            {
                Id = x.Recipient!.Id,
                Nickname = x.Recipient.Nickname,
                TelegramId = x.Recipient.TelegramId,
                IsVerified = x.Recipient.IsVerified
            },
            Sender = new UserDto() {
                Id = x.Sender!.Id,
                Nickname = x.Sender.Nickname,
                TelegramId = x.Sender.TelegramId,
                IsVerified = x.Sender.IsVerified
            },
            Type = x.Type,
            Title = x.Title,
            Content = x.Content,
            ExtraData = x.ExtraData
            
        })).Result;
    }
}