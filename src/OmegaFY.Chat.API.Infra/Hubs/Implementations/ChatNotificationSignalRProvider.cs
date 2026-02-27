using Microsoft.AspNetCore.SignalR;

namespace OmegaFY.Chat.API.Infra.Hubs.Implementations;

internal sealed class ChatNotificationSignalRProvider : IChatNotificationProvider
{
    private readonly IHubContext<ChatNotificationHub, IChatNotificationHub> _hubContext;

    public ChatNotificationSignalRProvider(IHubContext<ChatNotificationHub, IChatNotificationHub> hubContext) => _hubContext = hubContext;

    public async Task ConversationStartedAsync(Guid userId, Guid conversationId) => await _hubContext.Clients.User(userId.ToString()).ConversationStartedAsync(conversationId);

    public async Task MessageReceivedAsync(Guid userId, Guid conversationId, Guid messageId) => await _hubContext.Clients.User(userId.ToString()).MessageReceivedAsync(conversationId, messageId);

    public async Task FriendshipRequestReceivedAsync(Guid userId, Guid friendshipId) => await _hubContext.Clients.User(userId.ToString()).FriendshipRequestReceivedAsync(friendshipId);

    public async Task FriendshipRequestRejectedAsync(Guid userId, Guid friendshipId) => await _hubContext.Clients.User(userId.ToString()).FriendshipRequestRejectedAsync(friendshipId);

    public async Task FriendshipRemovedAsync(Guid userId, Guid friendshipId) => await _hubContext.Clients.User(userId.ToString()).FriendshipRemovedAsync(friendshipId);

    public async Task MemberAddedToGroupAsync(Guid userId, Guid conversationId) => await _hubContext.Clients.User(userId.ToString()).MemberAddedToGroupAsync(conversationId);

    public async Task MemberRemovedFromGroupAsync(Guid userId, Guid conversationId) => await _hubContext.Clients.User(userId.ToString()).MemberRemovedFromGroupAsync(conversationId);

    public async Task FriendLoggedInAsync(Guid userId, Guid friendUserId) => await _hubContext.Clients.User(userId.ToString()).FriendLoggedInAsync(friendUserId);

    public async Task FriendLoggedOffAsync(Guid userId, Guid friendUserId) => await _hubContext.Clients.User(userId.ToString()).FriendLoggedOffAsync(friendUserId);
}