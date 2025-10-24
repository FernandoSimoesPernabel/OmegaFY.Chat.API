using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Auth.Login;

internal sealed class NotifyThatFriendIsLoggedEventHandler : EventHandlerHandlerBase<UserLoggedInEvent>
{
    protected override Task HandleAsync(UserLoggedInEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}