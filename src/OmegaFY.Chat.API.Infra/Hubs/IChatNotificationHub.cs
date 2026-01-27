
namespace OmegaFY.Chat.API.Infra.Hubs;

internal interface IChatNotificationHub
{
    public Task ConversationStartedAsync(Guid conversationId);
}