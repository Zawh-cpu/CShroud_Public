using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Infrastructure.Data.Entities;

public enum FastLoginStatus
{
    Pending,
    Verified,
    Declined,
    Used
}

public class FastLogin
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [StringLength(15)]
    public string? Ipv4Address { get; set; }

    public string? Location { get; set; }

    [StringLength(225)]
    public string? DeviceInfo { get; set; }
    
    public required uint[] Variants { get; set; }
    
    [StringLength(256)]
    public required string Secret { get; set; }

    public FastLoginStatus Status { get; set; } = FastLoginStatus.Pending;
    public Guid VerifiedUserId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}