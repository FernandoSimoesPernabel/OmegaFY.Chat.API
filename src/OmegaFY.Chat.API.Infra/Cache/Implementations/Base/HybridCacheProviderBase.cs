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

    public async ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken)
    {
        //TODO telemetria
        return await InternalGetOrCreateAsync(key, factory, options, cancellationToken);
    }

    public abstract ValueTask RemoveAsync(string key, CancellationToken cancellationToken);

    public abstract ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken);

    public abstract ValueTask SetAsync<T>(string key, T value, CacheOptions options, CancellationToken cancellationToken);

    protected abstract ValueTask<T> InternalGetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken);
}
