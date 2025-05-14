using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Presentation.Api.v1.DataAnnotations;

public class TelegramSignUpRequest
{
    [Required] public required string FirstName { get; set; }
    [Required] public required string LastName { get; set; }
    [Required] public required long TelegramId { get; set; }
}