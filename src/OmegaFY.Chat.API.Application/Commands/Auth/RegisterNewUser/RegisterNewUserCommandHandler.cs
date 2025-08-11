using OmegaFY.Chat.API.Infra.MessageBus;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed class RegisterNewUserCommandHandler : CommandHandlerBase<RegisterNewUserCommandHandler, RegisterNewUserCommand, RegisterNewUserCommandResult>
{
    public RegisterNewUserCommandHandler(IMessageBus messageBus, IUserInformation currentUser, ILogger<RegisterNewUserCommandHandler> logger)
        : base(messageBus, currentUser, logger) { }

    public override Task<RegisterNewUserCommandResult> HandleAsync(RegisterNewUserCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(new RegisterNewUserCommandResult(Guid.NewGuid(), "", DateTime.Now, Guid.NewGuid(), DateTime.UtcNow));
    }
}