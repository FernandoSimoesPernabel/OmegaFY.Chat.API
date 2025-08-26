using OmegaFY.Chat.API.Infra.Cache.Constants;

namespace OmegaFY.Chat.API.Infra.Cache;

public static class CacheKeyGenerator
{
    public static string RefreshTokenKey(Guid userId, Guid refreshToken) => $"{CacheKeysConstants.REFRESH_TOKEN_PREFIX}_{userId}_{refreshToken}";
}