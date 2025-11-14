namespace OmegaFY.Chat.API.Application.Events.Chat.AddMemberToGroup;

public sealed record class MemberAddedToGroupEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public Guid MemberId { get; init; }

    public Guid UserId { get; init; }

    public MemberAddedToGroupEvent() { }

    public MemberAddedToGroupEvent(Guid conversationId, Guid memberId, Guid userId)
    {
        ConversationId = conversationId;
        MemberId = memberId;
        UserId = userId;
    }
}