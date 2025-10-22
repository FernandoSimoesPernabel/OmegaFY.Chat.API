using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

internal sealed class FriendshipAcceptedEventHandler : EventHandlerHandlerBase<FriendshipAcceptedEvent>
{
    protected override Task HandleAsync(FriendshipAcceptedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}