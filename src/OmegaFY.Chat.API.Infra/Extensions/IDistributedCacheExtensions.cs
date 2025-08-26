using Microsoft.Extensions.Caching.Distributed;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Cache;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class IDistributedCacheExtensions
{
    public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken)
    {
        var valueFromCache = await distributedCache.GetStringAsync(key, cancellationToken);
        return valueFromCache is null ? default : JsonSerializerHelper.Deserialize<T>(valueFromCache);
    }

    public static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken)
    {
        var valueAsJsonString = value.ToJson();
        return distributedCache.SetStringAsync(key, valueAsJsonString, options, cancellationToken);
    }

    public static Task SetAuthenticationTokenCacheAsync(
        this IDistributedCache distributedCache,
        Guid userId,
        AuthenticationToken authToken,
        CancellationToken cancellationToken)
    {
        return distributedCache.SetAsync(
           CacheKeyGenerator.RefreshTokenKey(userId, authToken.RefreshToken),
           authToken,
           new DistributedCacheEntryOptions() { AbsoluteExpiration = authToken.RefreshTokenExpirationDate },
           cancellationToken);
    }

    public static Task RemoveAuthenticationTokenCacheAsync(this IDistributedCache distributedCache, Guid userId, Guid refreshToken, CancellationToken cancellationToken)
        => distributedCache.RemoveAsync(CacheKeyGenerator.RefreshTokenKey(userId, refreshToken), cancellationToken);
}