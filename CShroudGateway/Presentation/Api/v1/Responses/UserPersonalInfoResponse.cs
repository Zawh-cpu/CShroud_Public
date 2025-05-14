using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Presentation.Api.v1.Responses;

public class UserPersonalInfoResponse
{
    public required Guid Id { get; set; }
    
    public string? Nickname { get; set; }
    
    public string? Login { get; set; }
    
    public long? TelegramId { get; set; }
    
    public required Role? Role { get; set; }
    
    public required Rate? Rate { get; set; }
    
    public DateTime? PayedUntil { get; set; }


    public required DateTime CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    
    public required bool IsVerified { get; set; }
}