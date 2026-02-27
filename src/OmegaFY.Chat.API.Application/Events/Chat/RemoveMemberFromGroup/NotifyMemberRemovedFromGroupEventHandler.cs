using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Chat.RemoveMemberFromGroup;

internal sealed class NotifyMemberRemovedFromGroupEventHandler : EventHandlerHandlerBase<MemberRemovedFromGroupEvent>
{
    private readonly IChatNotificationProvider _chatNotificationProvider;

    public NotifyMemberRemovedFromGroupEventHandler(IChatNotificationProvider chatNotificationProvider) => _chatNotificationProvider = chatNotificationProvider;

    protected async override Task HandleAsync(MemberRemovedFromGroupEvent @event, CancellationToken cancellationToken)
    {
        await _chatNotificationProvider.MemberRemovedFromGroupAsync(@event.UserId, @event.ConversationId);
    }
}