namespace CShroudGateway.Core.Extensions;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
    {
        userId = Guid.Empty;
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
            return false;

        return Guid.TryParse(userIdClaim.Value, out userId);
    }

}
