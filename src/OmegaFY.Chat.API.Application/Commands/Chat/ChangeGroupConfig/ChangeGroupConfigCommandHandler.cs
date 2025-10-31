using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Commands.Base;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

public sealed class ChangeGroupConfigCommandHandler : CommandHandlerBase<ChangeGroupConfigCommandHandler, ChangeGroupConfigCommand, ChangeGroupConfigCommandResult>
{
    public ChangeGroupConfigCommandHandler(
        IHostEnvironment hostEnvironment,
    IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<ChangeGroupConfigCommand> validator,
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<ChangeGroupConfigCommandResult>> InternalHandleAsync(ChangeGroupConfigCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
}
}