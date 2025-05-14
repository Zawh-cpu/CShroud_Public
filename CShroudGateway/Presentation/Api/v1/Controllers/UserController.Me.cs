using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class UserController
{

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMeAsync()
    {
        var user = await _userService.GetUserFromContext(User,
            x => x.Include(u => u.Role),
            x => x.Include(u => u.Rate));
        
        if (user == null) return BadRequest();
        
        return Ok(new UserPersonalInfoResponse()
        {
            Id = user.Id,
            Nickname = user.Nickname,
            Login = user.Login,
            TelegramId = user.TelegramId,
            Role = user.Role,
            Rate = user.Rate,
            PayedUntil = user.PayedUntil,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin,
            IsVerified = user.IsVerified
        });
    }
}