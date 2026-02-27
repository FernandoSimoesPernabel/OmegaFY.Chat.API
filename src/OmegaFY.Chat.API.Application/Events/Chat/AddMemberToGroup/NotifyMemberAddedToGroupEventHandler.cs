using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Chat.AddMemberToGroup;

internal sealed class NotifyMemberAddedToGroupEventHandler : EventHandlerHandlerBase<MemberAddedToGroupEvent>
{
    private readonly IChatNotificationProvider _chatNotificationProvider;

    public NotifyMemberAddedToGroupEventHandler(IChatNotificationProvider chatNotificationProvider) => _chatNotificationProvider = chatNotificationProvider;

    protected async override Task HandleAsync(MemberAddedToGroupEvent @event, CancellationToken cancellationToken)
    {
        await _chatNotificationProvider.MemberAddedToGroupAsync(@event.UserId, @event.ConversationId);
    }
}