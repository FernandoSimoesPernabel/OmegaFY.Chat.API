namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed record class GetMessageFromMemberQuery : IQuery
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public GetMessageFromMemberQuery() { }

    public GetMessageFromMemberQuery(Guid conversationId, Guid messageId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
    }
}