using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Tests.Integration.Mocks;

internal sealed class MockChatNotificationProvider : IChatNotificationProvider
{
    public Task ConversationStartedAsync(Guid userId, Guid conversationId) => Task.CompletedTask;

    public Task MessageReceivedAsync(Guid userId, Guid conversationId, Guid messageId) => Task.CompletedTask;

    public Task FriendshipRequestReceivedAsync(Guid userId, Guid friendshipId) => Task.CompletedTask;

    public Task FriendshipRequestRejectedAsync(Guid userId, Guid friendshipId) => Task.CompletedTask;

    public Task FriendshipRemovedAsync(Guid userId, Guid friendshipId) => Task.CompletedTask;

    public Task MemberAddedToGroupAsync(Guid userId, Guid conversationId) => Task.CompletedTask;

    public Task MemberRemovedFromGroupAsync(Guid userId, Guid conversationId) => Task.CompletedTask;

    public Task FriendLoggedInAsync(Guid userId, Guid friendUserId) => Task.CompletedTask;

    public Task FriendLoggedOffAsync(Guid userId, Guid friendUserId) => Task.CompletedTask;
}