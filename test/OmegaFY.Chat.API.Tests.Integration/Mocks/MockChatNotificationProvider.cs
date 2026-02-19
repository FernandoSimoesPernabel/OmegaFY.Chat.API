using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Tests.Integration.Mocks;

internal sealed class MockChatNotificationProvider : IChatNotificationProvider
{
    public Task ConversationStartedAsync(Guid userId, Guid conversationId) => Task.CompletedTask;
}