using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

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

    public async ValueTask<(bool cacheHit, T result)> GetOrDefaultAsync<T>(string key, CancellationToken cancellationToken)
        => await GetOrCreateAsync(key, (cancellationToken) => ValueTask.FromResult(default(T)), new CacheOptions(), cancellationToken);

    public async ValueTask<(bool cacheHit, T result)> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(factory);

        using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_HYBRID_CACHE_PROVIDER_NAME);

        activity.SetCacheKey(key);
        activity.SetCacheTags(options.Tags);

        _logger.LogInformation("Getting or creating cache entry for key: {CacheKey}", key);

        bool cacheHit = true;
        Activity parentActivity = Activity.Current;

        T result = await InternalGetOrCreateAsync(
            key,
            async (ct) =>
            {
                try
                {
                    Activity.Current ??= parentActivity;

                    cacheHit = false;
                    return await factory(ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating cache entry for key: {CacheKey}", key);

                    activity.SetErrorStatus(ex);

                    throw;
                }
            },
            options,
            cancellationToken);

        _logger.LogInformation("Cache entry for key: {CacheKey} retrieved. Cache hit: {CacheHit}", key, cacheHit);

        activity.SetCacheHit(cacheHit);
        activity.SetOkStatus();

        return (cacheHit, result);
    }

    public abstract ValueTask RemoveAsync(string key, CancellationToken cancellationToken);

    public abstract ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken);

    public abstract ValueTask SetAsync<T>(string key, T value, CacheOptions options, CancellationToken cancellationToken);

    protected abstract ValueTask<T> InternalGetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken);
}
