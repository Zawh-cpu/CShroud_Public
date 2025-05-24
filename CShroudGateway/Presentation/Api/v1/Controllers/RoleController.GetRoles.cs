using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class RoleController
{
    [HttpGet("roles")]
    public async Task<IActionResult> GetRolesAsync()
    {
        var roles = await _baseRepository.GetRolesByExpressionAsync(r => true);
        return new OkObjectResult(roles.Select(r => new
        {
            Id = r.Id,
            Name = r.Name
        }));
    }
}