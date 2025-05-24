using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Presentation.Api.v1.DataAnnotations;

public class PatchUserRequest
{
    [MaxLength(96)]
    public string? Nickname { get; set; }
    
    [MaxLength(96)]
    public string? Login { get; set; }
    
    [MaxLength(128)]
    public string? Password { get; set; }
    
    public uint? RoleId { get; set; }
    public uint? RateId { get; set; }
    public DateTime? RatePayedUntil { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsVerified { get; set; }
}