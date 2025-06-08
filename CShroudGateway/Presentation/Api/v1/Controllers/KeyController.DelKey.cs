using System.Security.Claims;
using Ardalis.Result;
using CShroudGateway.Core.Constants;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class KeyController
{
    [HttpDelete("{keyId:guid}")]
    [Authorize]
    public async Task<IActionResult> DelKeyAsync(Guid keyId)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role));
        if (user?.Role is null)
            return OftenErrors.InvalidUser;
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key?.Server is null || (key.UserId != user.Id && !user.HasAccess(UserRights.DeleteKeys)))
            return OftenErrors.KeyNotFound;

        await _keyService.DelKeyAsync(key);
        
        return Ok();
    }
}