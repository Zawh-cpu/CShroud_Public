﻿using System.Security.Claims;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using CShroudGateway.Presentation.Api.v1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class AuthController
{
    [HttpGet("fast_login")]
    public async Task<IActionResult> FastLoginAsync()
    {
        var session = _fastLoginService.MakeSession(Request.Headers["User-Agent"].ToString(),
            HttpContext.Connection.RemoteIpAddress?.ToString());
        
        await _baseRepository.AddEntityAsync(session);
        return new OkObjectResult(new FastLoginStartResponse()
        {
            Id = session.Id,
            ValidVariant = session.Variants[0],
            Secret = session.Secret,
        });
    }
}