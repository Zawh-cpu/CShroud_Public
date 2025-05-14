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
    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> AddKeyAsync([FromBody] AddKeyRequest request)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Rate));
        if (user is null) return OftenErrors.InvalidUser;
        
        var server = await _vpnServerManager.GetAvailableServerAsync("frankfurt", request.Protocol);
        if (server is null) return OftenErrors.ServerNotFound;

        var key = new Key()
        {
            Id = Guid.NewGuid(),
            ServerId = server.Id,
            Server = server,
            UserId = user.Id,
            User = user,
            Protocol = request.Protocol,
            Name = request.Name
        };
        
        var result = await _keyService.AddKeyAsync(key, user);
        if (!result.IsSuccess)
        {
            if (result.IsForbidden())
                return OftenErrors.MaxKeys;
            return OftenErrors.DawServerUnavailable;
        }
        
        return Ok(new AddKeyResponse()
        {   
            Id = key.Id
        });
    }
}