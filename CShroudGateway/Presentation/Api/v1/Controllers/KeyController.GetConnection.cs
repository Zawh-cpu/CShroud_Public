using System.Security.Claims;
using Ardalis.Result;
using CShroudGateway.Application.Factories;
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
    [HttpGet("{keyId:guid}/connect")]
    [Authorize]
    public async Task<IActionResult> GetConnectionAsync(Guid keyId)
    {
        var user = await _userService.GetUserFromContext(User, x => x.Include(u => u.Role));
        if (user?.Role is null)
            return OftenErrors.InvalidUser;
        
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key?.Server is null || (key.UserId != user.Id && !user.HasAccess(UserRights.ManageKeys)))
            return OftenErrors.KeyNotFound;

        var sets = await _baseRepository.GetProtocolSettingsAsync(p => p.ServerId == key.ServerId && p.Protocol == key.Protocol);
        if (sets is null)
            return OftenErrors.ProtocolNotFound;

        var creds = ProtocolConnectionFactory.CreateCredentials(sets, key, key.Server);
        Console.WriteLine(creds);
        
        return Ok(new KeyConnectionResponse()
        {   
            Id = key.Id,
            Protocol = key.Protocol,
            Credentials = creds
        });
    }
}