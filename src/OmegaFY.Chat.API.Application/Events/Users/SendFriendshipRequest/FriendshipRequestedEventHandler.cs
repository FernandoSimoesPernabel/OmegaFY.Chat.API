using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Users.SendFriendshipRequest;

internal sealed class FriendshipRequestedEventHandler : EventHandlerHandlerBase<FriendshipRequestedEvent>
{
    protected override Task HandleAsync(FriendshipRequestedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}