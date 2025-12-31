using OmegaFY.Chat.API.Infra.Constants;

namespace OmegaFY.Chat.API.Infra.Cache.Helpers;

public static class CacheKeyGenerator
{
    public static string RefreshTokenKey(Guid userId, string refreshToken) => $"{CacheKeysConstants.REFRESH_TOKEN_PREFIX}_{userId}_{refreshToken}";
}