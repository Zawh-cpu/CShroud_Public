using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class TelegramController
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] TelegramSignInRequest request)
    {
        var user = await _baseRepository.GetUserByExpressionAsync(u => u.TelegramId == request.TelegramId);
        if (user is null) return Unauthorized();
        
        var authDto = await _authService.SignInAsync(user, Request.Headers["User-Agent"].ToString(), HttpContext.Connection.RemoteIpAddress?.ToString());
        if (!authDto.IsSuccess) return new UnauthorizedResult();
        
        return new OkObjectResult(new SignInResponse
        {
            UserId = authDto.Value.UserId,
            ActionToken = authDto.Value.ActionToken,
            RefreshToken = authDto.Value.RefreshToken
        });
    }
}