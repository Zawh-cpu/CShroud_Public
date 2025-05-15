using System.Security.Claims;
using Ardalis.Result;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class KeyController
{
    [HttpGet("{keyId:guid}/switch/enable")]
    [Authorize]
    public async Task<IActionResult> EnableKeyAsync(Guid keyId)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role).Include(u => u.Rate));
        if (user?.Role is null)
            return OftenErrors.InvalidUser;
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key?.Server is null || (key.UserId != user.Id && !user.HasAccess(UserRights.ManageKeys)))
            return OftenErrors.KeyNotFound;
        
        if (key.Status == KeyStatus.Enabled)
            return Ok();
        
        if (!(await _keyService.EnableKeyAsync(key, user)).IsSuccess)
            return Forbid();
        
        return Ok();
    }
}