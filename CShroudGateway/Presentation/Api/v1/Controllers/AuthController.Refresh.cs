using System.Security.Claims;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpGet("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh()
    {
        if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId)) return BadRequest();

        var token = _tokenService.GenerateJwtToken(userId, TokenType.Action);
        await _baseRepository.AddEntityAsync(token);
        
        return new OkObjectResult(new AuthRefreshTokenResponse()
        {
            UserId = userId,
            ActionToken = token.TokenData,
        });
    }
}