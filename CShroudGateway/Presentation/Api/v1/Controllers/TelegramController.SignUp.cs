using Ardalis.Result;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class TelegramController
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] TelegramSignUpRequest request)
    {
        if (await _baseRepository.IsUserWithThisExpressionExistsAsync(u => u.TelegramId == request.TelegramId))
            return Conflict();

        var user = new User()
        {
            TelegramId = request.TelegramId,
            Nickname = $"{request.FirstName} {request.LastName}"
        };

        await _baseRepository.AddEntityAsync(user);
        
        var result = await _authService.SignInAsync(user, Request.Headers["User-Agent"].ToString(), HttpContext.Connection.RemoteIpAddress?.ToString());
        if (result.IsConflict())
        {
            return Conflict();
        }
        
        var res = result.Value;
        return new OkObjectResult(new
        {
            res.UserId,
            res.RefreshToken,
            res.ActionToken
        });
    }
}