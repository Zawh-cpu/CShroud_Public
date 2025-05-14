using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Presentation.Api.v1.DataAnnotations;

public class TelegramSignInRequest
{
    [Required]
    public required long TelegramId { get; set; }
}