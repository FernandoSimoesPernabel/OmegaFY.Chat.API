using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.MarkMessageAsDeleted;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;

public sealed class MarkMessageAsDeletedCommandHandler : CommandHandlerBase<MarkMessageAsDeletedCommandHandler, MarkMessageAsDeletedCommand, MarkMessageAsDeletedCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IConversationRepository _conversationRepository;

    private readonly IMemberMessageRepository _memberMessageRepository;

    public MarkMessageAsDeletedCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<MarkMessageAsDeletedCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IConversationRepository conversationRepository,
        IMemberMessageRepository memberMessageRepository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _conversationRepository = conversationRepository;
        _memberMessageRepository = memberMessageRepository;
    }

    protected async override Task<HandlerResult<MarkMessageAsDeletedCommandResult>> InternalHandleAsync(MarkMessageAsDeletedCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<MarkMessageAsDeletedCommandResult>();

        Member userMember = await _conversationRepository.GetMemberAsync(request.ConversationId, _userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (userMember is null)
            return HandlerResult.CreateNotFound<MarkMessageAsDeletedCommandResult>();

        MemberMessage memberMessage = await _memberMessageRepository.GetMemberMessageAsync(request.MessageId, userMember.Id, cancellationToken);

        if (memberMessage is null)
            return HandlerResult.CreateNotFound<MarkMessageAsDeletedCommandResult>();

        memberMessage.Deleted();

        await _memberMessageRepository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(new MessageDeletedEvent(userMember.ConversationId, memberMessage.MessageId, memberMessage.Id, userMember.UserId), cancellationToken);

        return HandlerResult.Create(new MarkMessageAsDeletedCommandResult());
    }
}