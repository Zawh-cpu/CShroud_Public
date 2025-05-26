using CShroudGateway.Core.Constants;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class UserController
{

    [HttpPatch("{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> PatchByIdAsync(Guid userId, [FromBody] PatchUserRequest request)
    {
        if (ReservedUsers.IsReserved(userId)) return OftenErrors.InvalidUser;
        
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role));
        if (user?.Role is null || (user.Id != userId && !user.HasAccess(UserRights.ModifyUsers)))
            return OftenErrors.InvalidUser;
        
        var lookedUser = await _baseRepository.GetUserByIdAsync(userId, x => x.Include(k => k.Role).Include(k => k.Rate));
        if (lookedUser is null) return OftenErrors.InvalidUser;
        
        if (request.Nickname is not null)
            lookedUser.Nickname = request.Nickname;
        
        if (request.Login is not null && lookedUser.Login is null)
            lookedUser.Login = request.Login;
        
        if (request.Password is not null)
            lookedUser.Password = request.Password;
        
        if (request.RoleId is not null)
            lookedUser.RoleId = request.RoleId.Value;

        if (request.RateId is not null && request.RatePayedUntil is not null && request.RatePayedUntil.Value.Kind == DateTimeKind.Utc)
        {
            lookedUser.RateId = request.RateId.Value;
            lookedUser.PayedUntil = request.RatePayedUntil.Value;
        }
        
        if (request.IsActive is not null && userId != user.Id)
            lookedUser.IsActive = request.IsActive.Value;
        
        if (request.IsVerified is not null)
            lookedUser.IsVerified = request.IsVerified.Value;

        lookedUser.UpdatedAt = DateTime.UtcNow;
        await _baseRepository.SaveContextAsync();
        return Ok();
    }
}