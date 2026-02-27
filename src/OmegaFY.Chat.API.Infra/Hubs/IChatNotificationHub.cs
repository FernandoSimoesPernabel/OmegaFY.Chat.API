
namespace OmegaFY.Chat.API.Infra.Hubs;

internal interface IChatNotificationHub
{
    public Task ConversationStartedAsync(Guid conversationId);

    public Task MessageReceivedAsync(Guid conversationId, Guid messageId);

    public Task FriendshipRequestReceivedAsync(Guid friendshipId);

    public Task FriendshipRequestRejectedAsync(Guid friendshipId);

    public Task FriendshipRemovedAsync(Guid friendshipId);

    public Task MemberAddedToGroupAsync(Guid conversationId);

    public Task MemberRemovedFromGroupAsync(Guid conversationId);

    public Task FriendLoggedInAsync(Guid friendUserId);

    public Task FriendLoggedOffAsync(Guid friendUserId);
}