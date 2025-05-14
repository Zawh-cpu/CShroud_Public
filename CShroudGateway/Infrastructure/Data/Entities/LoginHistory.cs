using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Infrastructure.Data.Entities;

public class LoginHistory
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public required Guid UserId { get; set; }
    
    [Required]
    public required Guid TokenId { get; set; }
    
    [StringLength(15)]
    public required string? Ipv4Address { get; set; }
    
    public required DateTime LoginTimeStamp { get; set; }
    
    [StringLength(225)]
    public string? DeviceInfo { get; set; }
    
    public User? User { get; set; }
    public Token? Token { get; set; }
}