
namespace OmegaFY.Chat.API.Infra.Hubs;

public interface IChatNotificationProvider
{
    public Task ConversationStartedAsync(Guid userId, Guid conversationId);
}