using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Cache;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class HybridCacheExtensions
{
    public static async ValueTask<T> GetOrDefaultAsync<T>(this HybridCache hybridCache, string key, CancellationToken cancellationToken)
    {
        return await hybridCache.GetOrCreateAsync(
            key,
            (cancellationToken) => ValueTask.FromResult(default(T)),
            cancellationToken: cancellationToken);
    }

    public static ValueTask SetAuthenticationTokenCacheAsync(
        this HybridCache hybridCache,
        Guid userId,
        AuthenticationToken authToken,
        CancellationToken cancellationToken)
    {
        return hybridCache.SetAsync<AuthenticationToken?>(
           CacheKeyGenerator.RefreshTokenKey(userId, authToken.RefreshToken),
           authToken,
           new HybridCacheEntryOptions()
           {
               Expiration = authToken.RefreshTokenExpirationDate - DateTime.UtcNow,
               LocalCacheExpiration = authToken.RefreshTokenExpirationDate - DateTime.UtcNow
           },
           [],
           cancellationToken);
    }

    public static ValueTask RemoveAuthenticationTokenCacheAsync(this HybridCache hybridCache, Guid userId, string refreshToken, CancellationToken cancellationToken)
        => hybridCache.RemoveAsync(CacheKeyGenerator.RefreshTokenKey(userId, refreshToken), cancellationToken);
}