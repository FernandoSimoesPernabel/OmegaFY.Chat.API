using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Commands.Base;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed class AddMemberToGroupCommandHandler : CommandHandlerBase<AddMemberToGroupCommandHandler, AddMemberToGroupCommand, AddMemberToGroupCommandResult>
{
    public AddMemberToGroupCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
IValidator<AddMemberToGroupCommand> validator,
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

  protected override Task<HandlerResult<AddMemberToGroupCommandResult>> InternalHandleAsync(AddMemberToGroupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}