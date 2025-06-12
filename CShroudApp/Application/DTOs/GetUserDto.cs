using CShroudApp.Core.Entities.User;

namespace CShroudApp.Application.DTOs;

public class GetUserDto
{
    public required Guid Id { get; set; }
    public required string Nickname { get; set; }
    public required Role Role { get; set; }
    public required Rate Rate { get; set; }
    public required bool IsVerified { get; set; }
    
    //JsonExtensionData]
    //public JsonObject ExternalData { get; set; }
}