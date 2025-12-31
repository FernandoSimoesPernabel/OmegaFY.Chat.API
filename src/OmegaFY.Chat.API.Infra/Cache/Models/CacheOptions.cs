namespace OmegaFY.Chat.API.Infra.Cache.Models;

public sealed class CacheOptions
{
    public string[] Tags { get; set; } = [];

    public TimeSpan? Expiration { get; init; }

    public TimeSpan? LocalCacheExpiration { get; set; }
}
