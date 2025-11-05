using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;

public sealed class MarkMessageAsReadCommandHandler : CommandHandlerBase<MarkMessageAsReadCommandHandler, MarkMessageAsReadCommand, MarkMessageAsReadCommandResult>
{
    public MarkMessageAsReadCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<MarkMessageAsReadCommand> validator,
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<MarkMessageAsReadCommandResult>> InternalHandleAsync(MarkMessageAsReadCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}