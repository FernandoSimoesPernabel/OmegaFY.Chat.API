using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed class RegisterNewUserCommandHandler : CommandHandlerBase<RegisterNewUserCommandHandler, RegisterNewUserCommand, RegisterNewUserCommandResult>
{
    public RegisterNewUserCommandHandler(IMessageBus messageBus, IHostEnvironment hostEnvironment) : base(messageBus, hostEnvironment) { }

    protected override Task<HandlerResult<RegisterNewUserCommandResult>> InternalHandleAsync(RegisterNewUserCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HandlerResult<RegisterNewUserCommandResult>(new RegisterNewUserCommandResult(Guid.NewGuid(), "", DateTime.Now, Guid.NewGuid(), DateTime.UtcNow)));
    }
}