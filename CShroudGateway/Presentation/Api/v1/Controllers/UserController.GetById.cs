using CShroudGateway.Core.Constants;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class UserController
{

    [HttpGet("{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync(Guid userId)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role));
        if (user?.Role is null || (user.Id != userId && !user.HasAccess(UserRights.AdminAccess)))
            return OftenErrors.InvalidUser;
        
        var lookedUser = await _baseRepository.GetUserByIdAsync(userId, x => x.Include(k => k.Role).Include(k => k.Rate));
        if (lookedUser?.Rate is null || lookedUser.Role is null)
            return OftenErrors.InvalidUser;
        
        return Ok(new UserPersonalInfoResponse()
        {
            Id = lookedUser.Id,
            Nickname = lookedUser.Nickname,
            Login = lookedUser.Login,
            TelegramId = lookedUser.TelegramId,
            Role = lookedUser.Role,
            Rate = lookedUser.Rate,
            PayedUntil = lookedUser.PayedUntil,
            CreatedAt = lookedUser.CreatedAt,
            LastLogin = lookedUser.LastLogin,
            IsVerified = lookedUser.IsVerified
        });
    }
}