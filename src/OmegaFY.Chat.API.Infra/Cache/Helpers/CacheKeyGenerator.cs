namespace OmegaFY.Chat.API.Infra.Cache.Helpers;

public static class CacheKeyGenerator
{
    public static string RefreshTokenKey(Guid userId, string refreshToken) => $"auth:refresh-token:{userId}:{refreshToken}";

    public static string UserIsLoggedInKey(Guid userId) => UserIsLoggedInKey(userId.ToString());

    public static string UserIsLoggedInKey(string userId) => $"auth:logged-in:{userId}";

    public static string CurrentUserInfoKey(Guid userId) => $"users:current:{userId}";

    public static string UserByIdKey(Guid userId) => $"users:by-id:{userId}";

    public static string FriendshipByIdKey(Guid userId, Guid friendshipId) => $"users:friendship:{userId}:{friendshipId}";

    public static string ConversationByIdKey(Guid conversationId, Guid userId) => $"chat:conversation:{conversationId}:user:{userId}";

    public static string MessageFromMemberKey(Guid conversationId, Guid messageId, Guid userId) => $"chat:conversation:{conversationId}:message:{messageId}:user:{userId}";

    public static string UserConversationsKey(Guid userId) => $"chat:user:{userId}:conversations";

    public static string UserConversationMessagesKey(Guid conversationId, Guid userId, int take, DateTime? cursor)
        => $"chat:conversation:{conversationId}:user:{userId}:messages:take:{take}:{(cursor.HasValue ? $"cursor:{cursor.Value:O}" : "cursor:none")}";
}