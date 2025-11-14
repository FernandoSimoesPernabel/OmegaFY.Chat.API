namespace OmegaFY.Chat.API.Application.Events.Chat.MarkMessageAsRead;

public sealed record MessageReadEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public Guid MemberId { get; init; }

    public Guid UserId { get; init; }

    public MessageReadEvent() { }

    public MessageReadEvent(Guid conversationId, Guid messageId, Guid memberId, Guid userId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
        MemberId = memberId;
        UserId = userId;
    }
}