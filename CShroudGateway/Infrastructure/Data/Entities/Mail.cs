using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CShroudGateway.Infrastructure.Data.Entities;

public enum MailType
{
    Message,
    RateExpiration,
    RateExpired
}

public class Mail
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [ForeignKey(nameof(Recipient))]
    public Guid? RecipientId { get; set; }
    public User? Recipient { get; set; }

    [ForeignKey(nameof(Sender))]
    public Guid SenderId { get; set; }
    
    public User? Sender { get; set; }

    public MailType Type { get; set; } = MailType.Message;
    
    [MaxLength(100)]
    public string? Title { get; set; }
    
    [MaxLength(500)]
    public string? Content { get; set; }
    
    public JsonDocument? ExtraData { get; set; }
    
    public bool IsRead { get; set; } = false;
    
    
}