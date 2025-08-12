using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;

namespace OmegaFY.Chat.API.Application.Commands.Base;

public abstract class CommandHandlerBase<TCommandHandler, TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    protected readonly IMessageBus _messageBus;

    protected readonly IHostEnvironment _hostEnvironment;

    protected CommandHandlerBase(IMessageBus messageBus, IHostEnvironment hostEnvironment)
    {
        _messageBus = messageBus;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<HandlerResult<TCommandResult>> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //TODO aplicar validação

            return await InternalHandleAsync(command, cancellationToken);
        }
        catch (ErrorCodeException ex)
        {
            return new HandlerResult<TCommandResult>(ex.ErrorCode, ex.Message);
        }
        catch (Exception ex)
        {
            return new HandlerResult<TCommandResult>(ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR, ex.GetSafeErrorMessageWhenInProd(_hostEnvironment.IsDevelopment()));
        }
    }

    protected abstract Task<HandlerResult<TCommandResult>> InternalHandleAsync(TCommand command, CancellationToken cancellationToken);
}