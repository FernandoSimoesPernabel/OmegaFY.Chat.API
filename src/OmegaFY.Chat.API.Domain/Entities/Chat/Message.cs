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

    public MessageBody Content { get; }

    public Message(ReferenceId conversationId, ReferenceId senderMemberId, MessageType messageType, MessageBody content)
    {
        //TODO
        ConversationId = conversationId;
        SenderMemberId = senderMemberId;
        Type = messageType;
        Content = content;

        SendDate = DateTime.UtcNow;
    }
}