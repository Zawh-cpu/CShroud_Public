using System.Security.Claims;
using Ardalis.Result;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Extensions;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class UserController
{
    [HttpDelete("{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> DelUserAsync(Guid userId)
    {
        if (!User.TryGetUserId(out Guid meId) || userId == meId || ReservedUsers.IsReserved(userId))
            return OftenErrors.InvalidUser;
        
        
        var user = await _baseRepository.GetUserByIdAsync(meId, x => x.Include(u => u.Role));
        if (user?.Role is null || !user.HasAccess(UserRights.DeleteUsers))
            return OftenErrors.InvalidUser;
        
        // NEEDS TO DISABLE THE ENTIRE STUFF THAT BELONGING TO THIS USER
        var lookedUser = await _baseRepository.GetUserByIdAsync(userId);
        if (lookedUser is not null)
            await _baseRepository.DelWithSaveAsync(lookedUser);
        
        return Ok();
    }
}