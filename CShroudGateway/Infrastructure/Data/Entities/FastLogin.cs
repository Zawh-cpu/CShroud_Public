using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Infrastructure.Data.Entities;

public class FastLogin
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public required Guid UserId { get; set; }
    
    [StringLength(15)]
    public required string? Ipv4Address { get; set; }
    public required DateTime LoginTimeStamp { get; set; }
    
    public required string Location { get; set; }
    [StringLength(225)]
    public string? DeviceInfo { get; set; }
    
    public required int[] Variants { get; set; }
}