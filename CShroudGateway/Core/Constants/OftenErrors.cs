using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Core.Constants;

public static class OftenErrors
{
    public static readonly NotFoundObjectResult ServerNotFound = new(new
    {
        Status = "error",
        Message = "This server isn't exists or maybe under maintenance."
    });
    
    public static readonly ForbidResult InvalidUser = new();
    
    
    public static readonly ObjectResult DawServerUnavailable = new(new
    {
        Status = "error",
        Message = "This DAW server is unavailable"
    })
    {
        StatusCode = 503
    };

    public static readonly ObjectResult MaxKeys = new(new
    {
        Status = "error",
        Message = "You've reached the maximum amount of keys"
    })
    {
        StatusCode = 403
    };

    public static readonly ObjectResult UnknownInternalError = new(new
    {
        Status = "error",
        Message = "Internal error or invalid arguments. Please, try later"
    })
    {
        StatusCode = 500
    };
}