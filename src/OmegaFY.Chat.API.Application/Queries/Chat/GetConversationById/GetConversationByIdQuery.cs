namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed record class GetConversationByIdQuery : IQuery
{
    public Guid ConversationId { get; init; }

    public GetConversationByIdQuery() { }

    public GetConversationByIdQuery(Guid conversationId) => ConversationId = conversationId;
}