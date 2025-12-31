using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class IHybridCacheProviderExtensions
{
    public static ValueTask SetAuthenticationTokenCacheAsync(
        this IHybridCacheProvider hybridCacheProvider,
        Guid userId,
        AuthenticationToken authToken,
        CancellationToken cancellationToken)
    {
        TimeSpan cacheTTL = authToken.RefreshTokenExpirationDate - DateTime.UtcNow;

        return hybridCacheProvider.SetAsync<AuthenticationToken?>(
           CacheKeyGenerator.RefreshTokenKey(userId, authToken.RefreshToken),
           authToken,
           new CacheOptions()
           {
               Expiration = cacheTTL,
               LocalCacheExpiration = cacheTTL,
               Tags = ["auth", "auth:refresh-token", $"auth:user:{userId}", $"user:{userId}"]
           },
           cancellationToken);
    }

    public static ValueTask RemoveAuthenticationTokenCacheAsync(this IHybridCacheProvider hybridCacheProvider, Guid userId, string refreshToken, CancellationToken cancellationToken)
        => hybridCacheProvider.RemoveAsync(CacheKeyGenerator.RefreshTokenKey(userId, refreshToken), cancellationToken);
}