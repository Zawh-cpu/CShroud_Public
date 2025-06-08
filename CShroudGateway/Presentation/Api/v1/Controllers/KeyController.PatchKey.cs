using System.Security.Claims;
using Ardalis.Result;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class KeyController
{
    [HttpPatch("{keyId:guid}")]
    [Authorize]
    public async Task<IActionResult> PatchKeyAsync(Guid keyId, [FromBody] PatchKeyRequest request)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role));
        if (user?.Role is null)
            return OftenErrors.InvalidUser;
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key?.Server is null || (key.UserId != user.Id && !user.HasAccess(UserRights.ManageKeys)))
            return OftenErrors.KeyNotFound;

        bool modified = false;

        if (request.Name is not null)
        {
            key.Name = request.Name;
            modified = true;
        }

        if (modified)
            await _baseRepository.SaveContextAsync();
        
        return Ok();
    }
}