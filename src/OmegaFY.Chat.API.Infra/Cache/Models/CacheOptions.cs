namespace OmegaFY.Chat.API.Infra.Cache.Models;

public sealed class CacheOptions
{
    public string[] Tags { get; init; } = [];

    public TimeSpan? Expiration { get; init; }

    public TimeSpan? LocalCacheExpiration { get; init; }
}
