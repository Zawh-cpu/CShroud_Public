using System.Security.Claims;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpPost("fast_login")]
    public async Task<IActionResult> FastLoginAsync([FromBody] FastLoginRequest request)
    {
        
        return Ok();
    }
}