namespace OmegaFY.Chat.API.Infra.Cache.Helpers;

public static class CacheKeyGenerator
{
    public static string RefreshTokenKey(Guid userId, string refreshToken) => $"auth:refresh-token:{userId}:{refreshToken}";

    public static string CurrentUserInfoKey(Guid userId) => $"users:current:{userId}";

    public static string UserByIdKey(Guid userId) => $"users:by-id:{userId}";

    public static string FriendshipByIdKey(Guid userId, Guid friendshipId) => $"users:friendship:{userId}:{friendshipId}";

    public static string ConversationByIdKey(Guid conversationId) => $"chat:conversation:{conversationId}";

    public static string MessageFromMemberKey(Guid conversationId, Guid messageId, Guid userId) => $"chat:conversation:{conversationId}:message:{messageId}:user:{userId}";

    public static string UserConversationsKey(Guid userId) => $"chat:user:{userId}:conversations";

    public static string UserConversationMessagesKey(Guid conversationId, Guid userId, int pageNumber, int pageSize)
        => $"chat:conversation:{conversationId}:user:{userId}:messages:page:{pageNumber}:size:{pageSize}";
}