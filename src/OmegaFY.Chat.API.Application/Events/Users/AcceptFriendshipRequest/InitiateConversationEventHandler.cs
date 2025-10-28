using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

internal sealed class InitiateConversationEventHandler : EventHandlerHandlerBase<FriendshipAcceptedEvent>
{
    protected override Task HandleAsync(FriendshipAcceptedEvent @event, CancellationToken cancellationToken)
    {
        //TODO criar uma conversação quando aceitar a amizade
        return Task.CompletedTask;
    }
}