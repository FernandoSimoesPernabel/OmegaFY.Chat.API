using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;

public sealed class SendMessageCommandHandler : CommandHandlerBase<SendMessageCommandHandler, SendMessageCommand, SendMessageCommandResult>
{
    private readonly IConversationRepository _conversationRepository;

    private readonly IMessageRepository _messageRepository;

    private readonly IUserInformation _userInformation;

    public SendMessageCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<SendMessageCommand> validator,
        IMessageBus messageBus,
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<SendMessageCommandResult>> InternalHandleAsync(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<SendMessageCommandResult>();

        Conversation conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<SendMessageCommandResult>();

        if (!conversation.IsMemberInConversation(_userInformation.CurrentRequestUserId.Value))
            return HandlerResult.CreateError<SendMessageCommandResult>(new DomainInvalidOperationException("Usu�rio n�o � membro da conversa."));

        Member senderMember = conversation.GetMemberByUserId(_userInformation.CurrentRequestUserId.Value);

        Message message = new Message(
            conversation.Id,
            senderMember.Id,
            request.Type,
            request.Body);

        await _messageRepository.CreateMessageAsync(message, cancellationToken);

        await _conversationRepository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(new MessageSentEvent(message.Id), cancellationToken);

        return HandlerResult.Create(new SendMessageCommandResult(message.Id));
    }
}