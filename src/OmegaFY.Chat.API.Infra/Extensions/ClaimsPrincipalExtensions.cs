using System.Security.Claims;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? TryGetUserIdFromClaims(this ClaimsPrincipal claimsPrincipal)
    {
        _ = Guid.TryParse(claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out Guid userId);
        return userId;
    }

    public static string TryGetEmailFromClaims(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

    public static bool IsAuthenticated(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Identity?.IsAuthenticated is true;
}