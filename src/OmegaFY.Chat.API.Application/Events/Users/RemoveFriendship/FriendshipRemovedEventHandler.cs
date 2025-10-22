using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Users.RemoveFriendship;

internal sealed class FriendshipRemovedEventHandler : EventHandlerHandlerBase<FriendshipRemovedEvent>
{
    protected override Task HandleAsync(FriendshipRemovedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}