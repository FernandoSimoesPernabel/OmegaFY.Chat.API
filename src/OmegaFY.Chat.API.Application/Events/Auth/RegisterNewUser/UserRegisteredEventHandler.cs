
namespace OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;

internal sealed class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    public Task HandleAsync(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}