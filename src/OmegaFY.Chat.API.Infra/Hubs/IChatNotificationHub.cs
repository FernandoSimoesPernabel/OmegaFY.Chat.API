
namespace OmegaFY.Chat.API.Infra.Hubs;

internal interface IChatNotificationHub
{
    public Task ConversationStarted(Guid conversationId);
}