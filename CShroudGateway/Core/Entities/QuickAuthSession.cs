using System.Security.Cryptography;
using System.Text;

namespace CShroudGateway.Core.Entities;

public enum QuickAuthStatus
{
    Pending,
    Confirmed,
    Declined,
    Used
}

public class QuickAuthSession
{
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public uint[] Variants { get; set; } = [(uint)RandomNumberGenerator.GetInt32(10, 999), (uint)RandomNumberGenerator.GetInt32(10, 999), (uint)RandomNumberGenerator.GetInt32(10, 999)];
    
    public uint ValidVariant;
    
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(15);
    public QuickAuthStatus Status { get; set; } = QuickAuthStatus.Pending;
    public Guid ConfirmedUserId { get; set; } = Guid.Empty;
    
    public string SecretLoginCode { get; set; } = string.Empty;

    public QuickAuthSession()
    {
        ValidVariant = Variants[RandomNumberGenerator.GetInt32(0, Variants.Length)];
    }
}