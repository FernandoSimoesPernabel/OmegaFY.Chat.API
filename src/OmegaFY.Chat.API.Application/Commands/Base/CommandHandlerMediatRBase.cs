using MediatR;

namespace OmegaFY.Chat.API.Application.Commands.Base;

public abstract class CommandHandlerMediatRBase<TCommandHandler, TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult>, ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand, IRequest<TCommandResult>
    where TCommandResult : ICommandResult
{
    protected readonly IUserInformation _currentUser;

    protected readonly ILogger<TCommandHandler> _logger;

    public CommandHandlerMediatRBase(IUserInformation currentUser, ILogger<TCommandHandler> logger)
    {
        _currentUser = currentUser;
        _logger = logger;
    }

    public Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken) => HandleAsync(request, cancellationToken);

    public abstract Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}