﻿using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.Repositories.Chat;

namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

internal class ReplicateMessageToMembersEventHandler : EventHandlerHandlerBase<MessageSentEvent>
{
    private readonly IConversationRepository _conversationRepository;

    private readonly IMessageRepository _messageRepository;

    public ReplicateMessageToMembersEventHandler(IConversationRepository conversationRepository, IMessageRepository messageRepository)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
    }

    protected override async Task HandleAsync(MessageSentEvent @event, CancellationToken cancellationToken)
    {
        Message message = await _messageRepository.GetByIdAsync(@event.MessageId, cancellationToken);

        Conversation conversation = await _conversationRepository.GetByIdAsync(message.ConversationId, cancellationToken);

        foreach (Member conversationMember in conversation.Members)
        {
            MemberMessage memberMessage = new MemberMessage(
                message.Id, 
                message.SenderMemberId,
                conversationMember.Id,
                MemberMessageStatus.Unread);

            await _messageRepository.CreateMemberMessageAsync(memberMessage, cancellationToken);
        }

        await _messageRepository.SaveChangesAsync(cancellationToken);

        //TODO SignalR de nova mensagem para os membros da conversa
    }
}