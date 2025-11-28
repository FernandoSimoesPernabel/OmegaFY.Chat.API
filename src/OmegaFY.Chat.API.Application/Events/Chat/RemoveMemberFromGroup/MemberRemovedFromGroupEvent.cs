namespace OmegaFY.Chat.API.Application.Events.Chat.RemoveMemberFromGroup;

public sealed record class MemberRemovedFromGroupEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public Guid MemberId { get; init; }

    public Guid UserId { get; init; }

    public MemberRemovedFromGroupEvent() { }

    public MemberRemovedFromGroupEvent(Guid conversationId, Guid memberId, Guid userId)
    {
        ConversationId = conversationId;
        MemberId = memberId;
        UserId = userId;
    }
}