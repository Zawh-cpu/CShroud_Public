using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        var user = await _baseRepository.GetUserByExpressionAsync(x => x.Login == request.Login && x.Password == request.Password);
        if (user is null) return new UnauthorizedResult();

        var authDto = await _authService.SignInAsync(user, Request.Headers["User-Agent"].ToString(),
            HttpContext.Connection.RemoteIpAddress?.ToString());
        if (!authDto.IsSuccess) return new UnauthorizedResult();
        
        return new OkObjectResult(new SignInResponse
        {
            UserId = authDto.Value.UserId,
            ActionToken = authDto.Value.ActionToken,
            RefreshToken = authDto.Value.RefreshToken
        });
    }
}