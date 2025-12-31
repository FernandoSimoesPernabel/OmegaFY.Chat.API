using Microsoft.Extensions.Caching.Hybrid;
using OmegaFY.Chat.API.Infra.Cache.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class CacheOptionsExtensions
{
    public static HybridCacheEntryOptions ToHybridCacheEntryOptions(this CacheOptions options)
    {
        return new HybridCacheEntryOptions()
        {
            Expiration = options.Expiration,
            LocalCacheExpiration = options.LocalCacheExpiration
        };
    }
}