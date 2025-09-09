using Microsoft.Extensions.Caching.Hybrid;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Cache;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class HybridCacheExtensions
{
    public static ValueTask SetAuthenticationTokenCacheAsync(
        this HybridCache hybridCache,
        Guid userId,
        AuthenticationToken authToken,
        CancellationToken cancellationToken)
    {
        return hybridCache.SetAsync(
           CacheKeyGenerator.RefreshTokenKey(userId, authToken.RefreshToken),
           authToken,
           new HybridCacheEntryOptions() { Expiration = authToken.RefreshTokenExpirationDate - DateTime.UtcNow },
           [userId.ToString()],
           cancellationToken);
    }
}