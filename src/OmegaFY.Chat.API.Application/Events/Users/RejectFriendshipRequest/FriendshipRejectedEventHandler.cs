using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;

internal sealed class FriendshipRejectedEventHandler : EventHandlerHandlerBase<FriendshipRejectedEvent>
{
    protected override Task HandleAsync(FriendshipRejectedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}