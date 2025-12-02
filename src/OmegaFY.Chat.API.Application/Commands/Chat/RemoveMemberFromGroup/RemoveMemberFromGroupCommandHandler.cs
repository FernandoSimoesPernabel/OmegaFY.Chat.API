using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.RemoveMemberFromGroup;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;

public sealed class RemoveMemberFromGroupCommandHandler : CommandHandlerBase<RemoveMemberFromGroupCommandHandler, RemoveMemberFromGroupCommand, RemoveMemberFromGroupCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IConversationRepository _repository;

    public RemoveMemberFromGroupCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RemoveMemberFromGroupCommand> validator,
        IMessageBus messageBus,
        ILogger<RemoveMemberFromGroupCommandHandler> logger,
        IUserInformation userInformation,
        IConversationRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus, logger)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected override async Task<HandlerResult<RemoveMemberFromGroupCommandResult>> InternalHandleAsync(RemoveMemberFromGroupCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<RemoveMemberFromGroupCommandResult>();

        Conversation conversation = await _repository.GetConversationByIdAsync(request.ConversationId, cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<RemoveMemberFromGroupCommandResult>();

        if (!conversation.IsUserInConversation(_userInformation.CurrentRequestUserId.Value))
            return HandlerResult.CreateUnauthorized<RemoveMemberFromGroupCommandResult>();

        Member memberRemoved = conversation.GetMemberByMemberId(request.MemberId);

        conversation.RemoveMemberFromGroup(request.MemberId);

        await _repository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(new MemberRemovedFromGroupEvent(conversation.Id, memberRemoved.Id, memberRemoved.UserId), cancellationToken);

        return HandlerResult.Create(new RemoveMemberFromGroupCommandResult());
    }
}