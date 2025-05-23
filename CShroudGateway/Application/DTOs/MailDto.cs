using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Nickname { get; set; }
    public long? TelegramId { get; set; }
    public bool IsVerified { get; set; }
}

public class MailDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required UserDto Recipient { get; set; }
    
    public required UserDto Sender { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MailType Type { get; set; } = MailType.Message;
    
    [MaxLength(100)]
    public string? Title { get; set; }
    
    [MaxLength(500)]
    public string? Content { get; set; }
    
    public JsonDocument? ExtraData { get; set; }
}