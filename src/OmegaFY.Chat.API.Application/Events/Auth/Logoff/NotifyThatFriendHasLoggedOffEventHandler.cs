using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Auth.Logoff;

internal sealed class NotifyThatFriendHasLoggedOffEventHandler : EventHandlerHandlerBase<UserLoggedOffEvent>
{
    protected override Task HandleAsync(UserLoggedOffEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}