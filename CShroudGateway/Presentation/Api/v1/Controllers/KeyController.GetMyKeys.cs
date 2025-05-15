using System.Security.Claims;
using CShroudGateway.Infrastructure.Data.Config;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class KeyController
{
    [HttpGet("")]
    public async Task<IActionResult> GetMyKeysAsync([FromQuery] int size = 0, [FromQuery] int page = 0)
    {
        if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid jti)) return NotFound();
        
        // THINK ABOUT OPTIMIZE
        
        var keys = await _baseRepository.GetKeysByExpressionAsync(key => key.UserId == jti, x => x.Include(k => k.Server));
        
        Response.Headers.Append("X-Total-Count", keys.Length.ToString());
        Response.Headers.Append("X-Enabled-Count", keys.Count(key => key.Status == KeyStatus.Enabled).ToString());
        
        return Ok(keys.OrderBy(k => k.Id).Skip(page * size).Take(size).Select(
            k => new KeyResponse()
            {
                Id = k.Id,
                Name = k.Name,
                Server = new ServerKeyResponse()
                {
                    Id = k.ServerId,
                    Location = k.Server!.Location,
                    Host = k.Server.IpV6Address != String.Empty ? k.Server.IpV6Address : k.Server.IpV4Address,
                    Port = (_protocolsConfig.Connections.TryGetValue(k.Protocol, out var conData) ? conData : new ProtocolsConfig.Protocol()).Port
                },
                Protocol = k.Protocol,
                UserId = k.UserId,
                CreatedAt = k.CreatedAt,
                Status = k.Status
            }
            ));
    }
}