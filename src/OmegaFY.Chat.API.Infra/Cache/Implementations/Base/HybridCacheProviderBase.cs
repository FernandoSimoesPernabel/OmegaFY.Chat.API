using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Infra.Cache.Implementations.Base;

internal abstract class HybridCacheProviderBase : IHybridCacheProvider
{
    protected readonly ILogger<HybridCacheProviderBase> _logger;

    protected readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    protected HybridCacheProviderBase(ILogger<HybridCacheProviderBase> logger, IOpenTelemetryRegisterProvider openTelemetryRegisterProvider)
    {
        _logger = logger;
        _openTelemetryRegisterProvider = openTelemetryRegisterProvider;
    }

    public async ValueTask<(bool cacheHit, T result)> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting or creating cache entry for key: {CacheKey}", key);

        bool cacheHit = true;

        T result = await InternalGetOrCreateAsync(key,
            async (ct) =>
            {
                try
                {
                    cacheHit = false;
                    return await factory(ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating cache entry for key: {CacheKey}", key);
                    throw;
                }
            },
            options,
            cancellationToken);

        _logger.LogInformation("Cache entry for key: {CacheKey} retrieved. Cache hit: {CacheHit}", key, cacheHit);

        return (cacheHit, result);
    }

    public abstract ValueTask RemoveAsync(string key, CancellationToken cancellationToken);

    public abstract ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken);

    public abstract ValueTask SetAsync<T>(string key, T value, CacheOptions options, CancellationToken cancellationToken);

    protected abstract ValueTask<T> InternalGetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken);
}
