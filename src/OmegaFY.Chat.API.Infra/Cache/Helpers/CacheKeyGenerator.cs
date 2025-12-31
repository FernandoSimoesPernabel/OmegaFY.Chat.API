namespace OmegaFY.Chat.API.Infra.Cache.Helpers;

public static class CacheKeyGenerator
{
    public static string RefreshTokenKey(Guid userId, string refreshToken) => $"auth:refresh-token:{userId}:{refreshToken}";
}