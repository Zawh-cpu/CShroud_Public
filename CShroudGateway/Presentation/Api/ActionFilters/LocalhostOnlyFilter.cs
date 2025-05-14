namespace CShroudGateway.Presentation.Api.ActionFilters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

public class LocalhostOnlyFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        if (remoteIp == null || !IPAddress.IsLoopback(remoteIp))
        {
            context.Result = new ContentResult
            {
                StatusCode = 403,
                Content = "Access denied"
            };
        }
    }
}
