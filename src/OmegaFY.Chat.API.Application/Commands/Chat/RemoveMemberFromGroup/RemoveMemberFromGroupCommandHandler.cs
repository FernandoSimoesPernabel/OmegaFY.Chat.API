using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Commands.Base;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;

public sealed class RemoveMemberFromGroupCommandHandler : CommandHandlerBase<RemoveMemberFromGroupCommandHandler, RemoveMemberFromGroupCommand, RemoveMemberFromGroupCommandResult>
{
    public RemoveMemberFromGroupCommandHandler(
  IHostEnvironment hostEnvironment,
   IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RemoveMemberFromGroupCommand> validator,
    IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<RemoveMemberFromGroupCommandResult>> InternalHandleAsync(RemoveMemberFromGroupCommand request, CancellationToken cancellationToken)
    {
   throw new NotImplementedException();
    }
}