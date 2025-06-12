namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

internal sealed class RegisterNewUserCommandHandler : CommandHandlerMediatRBase<RegisterNewUserCommandHandler, RegisterNewUserCommand, RegisterNewUserCommandResult>
{
    public RegisterNewUserCommandHandler(IUserInformation currentUser, ILogger<RegisterNewUserCommandHandler> logger) : base(currentUser, logger) { }

    public override Task<RegisterNewUserCommandResult> HandleAsync(RegisterNewUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}