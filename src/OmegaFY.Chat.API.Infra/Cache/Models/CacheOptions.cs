namespace OmegaFY.Chat.API.Infra.Cache.Models;

public sealed class CacheOptions
{
    public string[] Tags { get; init; } = [];

    public TimeSpan? Expiration { get; init; }

    public TimeSpan? LocalCacheExpiration { get; init; }

    public CacheOptions() { }

    public CacheOptions(TimeSpan? expiration) : this([], expiration) { }

    public CacheOptions(string[] tags, TimeSpan? expiration) : this(tags, expiration, expiration) { }

    public CacheOptions(string[] tags, TimeSpan? expiration, TimeSpan? localCacheExpiration)
    {
        Tags = tags;
        Expiration = expiration;
        LocalCacheExpiration = localCacheExpiration;
    }
}
