using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Presentation.Api.v1.DataAnnotations;

public class SignInRequest
{
    [Required]
    public required string Login { get; set; }
    
    [Required]
    public required string Password { get; set; }
}