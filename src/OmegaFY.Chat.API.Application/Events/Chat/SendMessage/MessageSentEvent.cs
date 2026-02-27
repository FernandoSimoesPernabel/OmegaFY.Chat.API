namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

public sealed record class MessageSentEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public Guid SenderUserId { get; init; }

    public MessageSentEvent() { }

    public MessageSentEvent(Guid conversationId, Guid messageId, Guid senderUserId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
        SenderUserId = senderUserId;
    }
}