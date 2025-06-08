using System.Security.Claims;
using Ardalis.Result;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class KeyController
{
    [HttpGet("{keyId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetMyKeyAsync(Guid keyId)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role));
        if (user?.Role is null)
            return OftenErrors.InvalidUser;
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key?.Server is null || (key.UserId != user.Id && !user.HasAccess(UserRights.KeyViewAccess)))
            return OftenErrors.KeyNotFound;

        if (!_protocolsConfig.Connections.TryGetValue(key.Protocol, out var protocolData))
            return OftenErrors.ProtocolNotFound;
        
        return Ok(new KeyResponse()
        {   
            Id = key.Id,
            Name = key.Name,
            Server = new ServerKeyResponse()
            {
                Id = key.ServerId,
                Location = key.Server.Location,
                Host = key.Server.IpV6Address != String.Empty ? key.Server.IpV6Address : key.Server.IpV4Address,
                Port = protocolData.Port
            },
            Protocol = key.Protocol,
            UserId = key.UserId,
            CreatedAt = key.CreatedAt,
            Status = key.Status
        });
    }
}