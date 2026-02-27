using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Users.SendFriendshipRequest;

internal sealed class FriendshipRequestedEventHandler : EventHandlerHandlerBase<FriendshipRequestedEvent>
{
    private readonly IChatNotificationProvider _chatNotificationProvider;

    public FriendshipRequestedEventHandler(IChatNotificationProvider chatNotificationProvider) => _chatNotificationProvider = chatNotificationProvider;

    protected async override Task HandleAsync(FriendshipRequestedEvent @event, CancellationToken cancellationToken)
    {
        await _chatNotificationProvider.FriendshipRequestReceivedAsync(@event.InvitedUserId, @event.FriendshipId);
    }
}