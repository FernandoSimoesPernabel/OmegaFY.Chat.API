using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class IHybridCacheProviderExtensions
{
    public static ValueTask SetAuthenticationTokenAsync(
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
               Tags = [CacheTagsGenerator.AuthTag(), CacheTagsGenerator.AuthRefreshTokenTag(), CacheTagsGenerator.AuthUserIdTag(userId), CacheTagsGenerator.UserIdTag(userId)]
           },
           cancellationToken);
    }

    public static ValueTask RemoveAuthenticationTokenAsync(this IHybridCacheProvider hybridCacheProvider, Guid userId, string refreshToken, CancellationToken cancellationToken)
        => hybridCacheProvider.RemoveAsync(CacheKeyGenerator.RefreshTokenKey(userId, refreshToken), cancellationToken);

    public static ValueTask SetUserIsLoggedInAsync(this IHybridCacheProvider hybridCacheProvider, string userId, CancellationToken cancellationToken)
    {
        return hybridCacheProvider.SetAsync(
            CacheKeyGenerator.UserIsLoggedInKey(userId),
            userId,
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.ONE_HOUR,
                LocalCacheExpiration = TimeSpanConstants.ONE_HOUR,
                Tags = [CacheTagsGenerator.AuthTag(), CacheTagsGenerator.AuthLoggedInTag(), CacheTagsGenerator.AuthUserIdTag(userId), CacheTagsGenerator.UserIdTag(userId)]
            },
            cancellationToken);
    }
}