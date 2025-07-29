using OmegaFY.Chat.API.Infra.MessageBus;

namespace OmegaFY.Chat.API.Application.Commands.Base;

public abstract class CommandHandlerBase<TCommandHandler, TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    protected readonly IMessageBus _messageBus;

    protected readonly IUserInformation _currentUser;

    protected readonly ILogger<TCommandHandler> _logger;

    protected CommandHandlerBase(IMessageBus messageBus, IUserInformation currentUser, ILogger<TCommandHandler> logger)
    {
        _messageBus = messageBus;
        _currentUser = currentUser;
        _logger = logger;
    }

    public Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken) => HandleAsync(request, cancellationToken);

    public abstract Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}