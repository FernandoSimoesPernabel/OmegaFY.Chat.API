using OmegaFY.Chat.API.Infra.Cache.Models;

namespace OmegaFY.Chat.API.Infra.Cache;

public interface IHybridCacheProvider
{
    public ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, CacheOptions options, CancellationToken cancellationToken);

    public ValueTask SetAsync<T>(string key, T value, CacheOptions options, CancellationToken cancellationToken);

    public ValueTask RemoveAsync(string key, CancellationToken cancellationToken);

    public ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken);
}