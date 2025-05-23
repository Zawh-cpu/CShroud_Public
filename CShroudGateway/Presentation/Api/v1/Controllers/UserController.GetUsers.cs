using CShroudGateway.Application.DTOs;
using CShroudGateway.Core.Constants;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class UserController
{

    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetUsersAsync([FromQuery] int size = 0, [FromQuery] int page = 0)
    {
        var user = await _userService.GetUserFromContext(User,
            x => x.Include(u => u.Role));
        if (user is null || !user.HasAccess(UserRights.AdminAccess))
            return OftenErrors.PermissionDenied;

        var users = await _baseRepository.GetUsersByExpressionAsync(x => true);
        Response.Headers.Append("X-Total-Count", users.Length.ToString());
        return Ok(users.Skip(page * size).Take(size).Select(u => new UserDto()
        {
            Id = u.Id,
            Nickname = u.Nickname,
            TelegramId = u.TelegramId,
            IsVerified = u.IsVerified
        }));
    }
}