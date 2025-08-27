using OmegaFY.Chat.API.Application.Events.Base;

namespace OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;

internal sealed class UserRegisteredEventHandler : EventHandlerHandlerBase<UserRegisteredEvent>
{
    protected override Task HandleAsync(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}