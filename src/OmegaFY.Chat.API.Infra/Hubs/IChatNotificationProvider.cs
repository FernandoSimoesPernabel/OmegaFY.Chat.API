namespace OmegaFY.Chat.API.Infra.Hubs;

public interface IChatNotificationProvider
{
    public Task ConversationStartedAsync(Guid userId, Guid conversationId);

    public Task MessageReceivedAsync(Guid userId, Guid conversationId, Guid messageId);

    public Task FriendshipRequestReceivedAsync(Guid userId, Guid friendshipId);

    public Task FriendshipRequestRejectedAsync(Guid userId, Guid friendshipId);

    public Task FriendshipRemovedAsync(Guid userId, Guid friendshipId);

    public Task MemberAddedToGroupAsync(Guid userId, Guid conversationId);

    public Task MemberRemovedFromGroupAsync(Guid userId, Guid conversationId);

    public Task FriendLoggedInAsync(Guid userId, Guid friendUserId);

    public Task FriendLoggedOffAsync(Guid userId, Guid friendUserId);
}