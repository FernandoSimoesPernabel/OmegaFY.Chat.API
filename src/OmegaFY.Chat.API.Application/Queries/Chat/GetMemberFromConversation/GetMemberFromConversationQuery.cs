namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;

public sealed record class GetMemberFromConversationQuery : IQuery
{
    public Guid ConversationId { get; init; }

    public Guid MemberId { get; init; }

    public GetMemberFromConversationQuery() { }

    public GetMemberFromConversationQuery(Guid conversationId, Guid memberId)
    {
        ConversationId = conversationId;
        MemberId = memberId;
    }
}