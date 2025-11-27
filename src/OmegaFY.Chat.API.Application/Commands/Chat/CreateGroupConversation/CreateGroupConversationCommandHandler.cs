using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.CreateGroupConversation;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;

public sealed class CreateGroupConversationCommandHandler : CommandHandlerBase<CreateGroupConversationCommandHandler, CreateGroupConversationCommand, CreateGroupConversationCommandResult>
{
    private readonly IConversationRepository _repository;

    private readonly IUserInformation _userInformation;

    public CreateGroupConversationCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<CreateGroupConversationCommand> validator,
        IMessageBus messageBus,
        IConversationRepository repository,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _repository = repository;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<CreateGroupConversationCommandResult>> InternalHandleAsync(CreateGroupConversationCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<CreateGroupConversationCommandResult>();

        Conversation newGroupConversation = Conversation.CreateGroupChat(_userInformation.CurrentRequestUserId.Value, request.GroupName, request.MaxNumberOfMembers);

        await _repository.CreateConversationAsync(newGroupConversation, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(new GroupConversationCreatedEvent(newGroupConversation.Id), cancellationToken);

        return HandlerResult.Create(new CreateGroupConversationCommandResult(newGroupConversation.Id));
    }
}