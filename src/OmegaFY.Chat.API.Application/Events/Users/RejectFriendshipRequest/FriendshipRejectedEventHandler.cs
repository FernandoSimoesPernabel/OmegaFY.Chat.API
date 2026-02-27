using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;

internal sealed class FriendshipRejectedEventHandler : EventHandlerHandlerBase<FriendshipRejectedEvent>
{
    private readonly IChatNotificationProvider _chatNotificationProvider;

    public FriendshipRejectedEventHandler(IChatNotificationProvider chatNotificationProvider) => _chatNotificationProvider = chatNotificationProvider;

    protected async override Task HandleAsync(FriendshipRejectedEvent @event, CancellationToken cancellationToken)
    {
        await _chatNotificationProvider.FriendshipRequestRejectedAsync(@event.RequestingUserId, @event.FriendshipId);
    }
}