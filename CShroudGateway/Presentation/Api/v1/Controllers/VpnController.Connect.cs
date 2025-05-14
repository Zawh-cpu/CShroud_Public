using CShroudGateway.Presentation.Api.v1.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class VpnController
{
    [HttpPost("connect")]
    public async Task ConnectAsync([FromBody] VpnConnectionRequest request)
    {
        await Task.CompletedTask;
        // if 
    }
}