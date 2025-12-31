using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Infra.Cache.Implementations.Base;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Infra.Cache.Implementations;

internal sealed class HybridCacheProvider : HybridCacheProviderBase
{
    private readonly HybridCache _hybridCache;

    public HybridCacheProvider(
        ILogger<HybridCacheProviderBase> logger,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        HybridCache hybridCache) : base(logger, openTelemetryRegisterProvider)
    {
        _hybridCache = hybridCache;
    }

    public override async ValueTask RemoveAsync(string key, CancellationToken cancellationToken) => await _hybridCache.RemoveAsync(key, cancellationToken);

    public override async ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken) => await _hybridCache.RemoveByTagAsync(tag, cancellationToken);

    public override async ValueTask SetAsync<T>(string key, T value, CacheOptions options, CancellationToken cancellationToken) 
        => await _hybridCache.SetAsync(key, value, options.ToHybridCacheEntryOptions(), options.Tags, cancellationToken);

    protected override async ValueTask<T> InternalGetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken) 
        => await _hybridCache.GetOrCreateAsync(key, factory, options.ToHybridCacheEntryOptions(), options.Tags, cancellationToken);
}