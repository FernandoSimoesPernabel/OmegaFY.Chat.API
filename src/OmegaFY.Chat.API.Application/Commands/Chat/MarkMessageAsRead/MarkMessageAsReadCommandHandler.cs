using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.MarkMessageAsRead;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;

public sealed class MarkMessageAsReadCommandHandler : CommandHandlerBase<MarkMessageAsReadCommandHandler, MarkMessageAsReadCommand, MarkMessageAsReadCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IConversationRepository _conversationRepository;

    private readonly IMessageRepository _messageRepository;

    public MarkMessageAsReadCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<MarkMessageAsReadCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
    }

    protected async override Task<HandlerResult<MarkMessageAsReadCommandResult>> InternalHandleAsync(MarkMessageAsReadCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<MarkMessageAsReadCommandResult>();

        Member userMember = await _conversationRepository.GetMemberAsync(request.ConversationId, _userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (userMember is null)
            return HandlerResult.CreateNotFound<MarkMessageAsReadCommandResult>();

        MemberMessage memberMessage = await _messageRepository.GetMemberMessageAsync(request.MessageId, userMember.Id, cancellationToken);

        if (memberMessage is null)
            return HandlerResult.CreateNotFound<MarkMessageAsReadCommandResult>();

        memberMessage.Read();

        await _messageRepository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(new MessageReadEvent(userMember.ConversationId, memberMessage.MessageId, memberMessage.Id, userMember.UserId), cancellationToken);

        return HandlerResult.Create(new MarkMessageAsReadCommandResult());
    }
}