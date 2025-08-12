namespace OmegaFY.Chat.API.Application.Commands;

public interface ICommandHandler<TCommand, TCommandResult> where TCommand : ICommand where TCommandResult : ICommandResult
{
    public Task<HandlerResult<TCommandResult>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}