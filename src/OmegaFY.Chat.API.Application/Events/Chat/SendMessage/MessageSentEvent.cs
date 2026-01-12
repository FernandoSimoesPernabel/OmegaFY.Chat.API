namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

public sealed record class MessageSentEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public MessageSentEvent() { }

    public MessageSentEvent(Guid conversationId, Guid messageId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
    }
}