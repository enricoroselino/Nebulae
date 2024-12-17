using System.Security.Claims;
using API.Shared.Exceptions;
using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Shared.Extensions;

public static class ClaimExtensions
{
    private static string? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(Match)?.Value;

        bool Match(Claim c) => c.Type is ClaimTypes.NameIdentifier or JwtRegisteredClaimNames.Sub;
    }

    private static string? GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(Match)?.Value;

        bool Match(Claim c) => c.Type is ClaimTypes.Name or JwtRegisteredClaimNames.Name;
    }

    public static (Guid UserId, string UserName) GetAuthenticatedUser(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();
        var userName = claimsPrincipal.GetUserName();

        if (string.IsNullOrWhiteSpace(userName) || !Guid.TryParse(userId, out var parsedId))
        {
            throw new UnauthorizedException();
        }

        return (parsedId, userName);
    }
}