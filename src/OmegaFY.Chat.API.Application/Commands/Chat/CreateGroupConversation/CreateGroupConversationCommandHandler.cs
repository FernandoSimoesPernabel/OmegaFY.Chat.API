using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Commands.Base;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;

public sealed class CreateGroupConversationCommandHandler : CommandHandlerBase<CreateGroupConversationCommandHandler, CreateGroupConversationCommand, CreateGroupConversationCommandResult>
{
    public CreateGroupConversationCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
    IValidator<CreateGroupConversationCommand> validator,
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<CreateGroupConversationCommandResult>> InternalHandleAsync(CreateGroupConversationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}