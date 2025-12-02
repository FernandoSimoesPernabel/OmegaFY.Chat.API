using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Base;

public abstract class CommandHandlerBase<TCommandHandler, TCommand, TCommandResult> : HandlerBase<TCommandHandler, TCommand, TCommandResult>, ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    protected readonly IMessageBus _messageBus;

    protected CommandHandlerBase(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<TCommand> validator,
        IMessageBus messageBus,
        ILogger<TCommandHandler> logger) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger) => _messageBus = messageBus;
}