using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;

internal sealed class SendWelcomeEmailEventHandler : EventHandlerHandlerBase<UserRegisteredEvent>
{
    protected override Task HandleAsync(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}