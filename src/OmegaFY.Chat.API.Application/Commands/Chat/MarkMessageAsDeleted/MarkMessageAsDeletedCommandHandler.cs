using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;

public sealed class MarkMessageAsDeletedCommandHandler : CommandHandlerBase<MarkMessageAsDeletedCommandHandler, MarkMessageAsDeletedCommand, MarkMessageAsDeletedCommandResult>
{
    public MarkMessageAsDeletedCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<MarkMessageAsDeletedCommand> validator,
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<MarkMessageAsDeletedCommandResult>> InternalHandleAsync(MarkMessageAsDeletedCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}