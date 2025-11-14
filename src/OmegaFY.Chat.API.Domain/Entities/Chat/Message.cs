using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class Message : Entity, IAggregateRoot<Message>
{
    public ReferenceId ConversationId { get; }

    public ReferenceId SenderMemberId { get; }

    public DateTime SendDate { get; }

    public MessageType Type { get; }

    public MessageBody Body { get; }

    internal Message() { }

    public Message(ReferenceId conversationId, ReferenceId senderMemberId, MessageType messageType, MessageBody body)
    {
        if (!messageType.IsDefined())
            throw new DomainArgumentException("Tipo de mensagem inválido.");

        ConversationId = conversationId;
        SenderMemberId = senderMemberId;
        Type = messageType;
        Body = body;

        SendDate = DateTime.UtcNow;
    }
}