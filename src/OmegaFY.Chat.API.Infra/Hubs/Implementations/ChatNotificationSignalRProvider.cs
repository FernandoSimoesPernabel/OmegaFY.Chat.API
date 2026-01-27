using Microsoft.AspNetCore.SignalR;

namespace OmegaFY.Chat.API.Infra.Hubs.Implementations;

internal sealed class ChatNotificationSignalRProvider : IChatNotificationProvider
{
    private readonly IHubContext<ChatNotificationHub, IChatNotificationHub> _hubContext;

    public ChatNotificationSignalRProvider(IHubContext<ChatNotificationHub, IChatNotificationHub> hubContext) => _hubContext = hubContext;

    public async Task ConversationStartedAsync(Guid userId, Guid conversationId) => await _hubContext.Clients.User(userId.ToString()).ConversationStartedAsync(conversationId);
}