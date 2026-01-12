using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.ChangeGroupConfig;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

public sealed class ChangeGroupConfigCommandHandler : CommandHandlerBase<ChangeGroupConfigCommandHandler, ChangeGroupConfigCommand, ChangeGroupConfigCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IConversationRepository _repository;

    public ChangeGroupConfigCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<ChangeGroupConfigCommand> validator,
        IMessageBus messageBus,
        ILogger<ChangeGroupConfigCommandHandler> logger,
        IUserInformation userInformation,
        IConversationRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus, logger)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected async override Task<HandlerResult<ChangeGroupConfigCommandResult>> InternalHandleAsync(ChangeGroupConfigCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<ChangeGroupConfigCommandResult>();

        Conversation conversation = await _repository.GetConversationByIdAsync(request.ConversationId, cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<ChangeGroupConfigCommandResult>();

        if (conversation.GroupConfig.CreatedByUserId.Value != _userInformation.CurrentRequestUserId.Value)
            return HandlerResult.CreateUnauthorized<ChangeGroupConfigCommandResult>();

        conversation.ChangeGroupConfig(request.NewGroupName, request.NewMaxNumberOfMembers);

        await _repository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(
            new GroupConfigChangedEvent(conversation.Id, conversation.GroupConfig.GroupName, conversation.GroupConfig.MaxNumberOfMembers, conversation.GetMembersUserIds()), 
            cancellationToken);

        return HandlerResult.Create(new ChangeGroupConfigCommandResult(
            conversation.Id, 
            conversation.GroupConfig.CreatedByUserId,
            conversation.GroupConfig.GroupName,
            conversation.GroupConfig.MaxNumberOfMembers));
    }
}