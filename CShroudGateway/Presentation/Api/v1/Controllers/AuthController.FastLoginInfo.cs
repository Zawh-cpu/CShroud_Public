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
    [HttpGet("fast_login/{flId:guid}/info")]
    public async Task<IActionResult> FastLoginInfoAsync(Guid flId)
    {
        var session = await _baseRepository.GetFastLoginByExpressionAsync(fl => fl.Id == flId);
        if (session is null || session.Status != FastLoginStatus.Pending || session.CreatedAt.AddMinutes(15) < DateTime.UtcNow) return NotFound();
        
        return new OkObjectResult(new FastLoginInfoResponse()
        {
            Id = session.Id,
            Variants = session.Variants
        });
    }
}