
namespace OmegaFY.Chat.API.Application.Events.Chat.MarkMessageAsDeleted;

public sealed record MessageDeletedEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public Guid MemberId { get; init; }

    public Guid UserId { get; init; }

    public MessageDeletedEvent() { }

    public MessageDeletedEvent(Guid conversationId, Guid messageId, Guid memberId, Guid userId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
        MemberId = memberId;
        UserId = userId;
    }
}