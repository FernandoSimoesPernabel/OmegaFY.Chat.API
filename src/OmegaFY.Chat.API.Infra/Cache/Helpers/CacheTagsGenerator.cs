namespace OmegaFY.Chat.API.Infra.Cache.Helpers;

public static class CacheTagsGenerator
{
    public static string ChatTag() => "chat";

    public static string ChatConversationsTag() => "chat:conversations";

    public static string ChatMessagesTag() => "chat:messages";

    public static string ChatConversationIdTag(Guid conversationId) => $"chat:conversation:{conversationId}";

    public static string ChatMessageIdTag(Guid messageId) => $"chat:message:{messageId}";

    public static string ChatUserIdTag(Guid userId) => $"chat:user:{userId}";

    public static string UsersTag() => "users";

    public static string UsersCurrentTag() => "users:current";

    public static string UsersByIdTag() => "users:by-id";

    public static string UsersFriendshipsTag() => "users:friendships";

    public static string UsersUserIdTag(Guid userId) => $"users:user:{userId}";

    public static string UserIdTag(Guid userId) => $"user:{userId}";

    public static string FriendshipIdTag(Guid friendshipId) => $"friendship:{friendshipId}";
}