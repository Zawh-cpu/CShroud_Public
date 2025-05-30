using System.Security.Claims;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpGet("fast_login/{flId:guid}")]
    public async Task<IActionResult> FastLoginCheckAsync(Guid flId)
    {
        var session = await _baseRepository.GetFastLoginByExpressionAsync(fl => fl.Id == flId);
        if (session is null || session.CreatedAt.AddMinutes(15) < DateTime.UtcNow) return NotFound();
        
        if (session.Status == FastLoginStatus.Verified)
            return Ok();
        
        return NoContent();
    }
}