using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Users.RemoveFriendship;

internal sealed class CloseConversationEventHandler : EventHandlerHandlerBase<FriendshipRemovedEvent>
{
    private readonly IChatNotificationProvider _chatNotificationProvider;

    public CloseConversationEventHandler(IChatNotificationProvider chatNotificationProvider) => _chatNotificationProvider = chatNotificationProvider;

    protected async override Task HandleAsync(FriendshipRemovedEvent @event, CancellationToken cancellationToken)
    {
        await _chatNotificationProvider.FriendshipRemovedAsync(@event.RequestingUserId, @event.FriendshipId);
        await _chatNotificationProvider.FriendshipRemovedAsync(@event.InvitedUserId, @event.FriendshipId);
    }
}