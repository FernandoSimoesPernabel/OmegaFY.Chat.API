namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed record GetUserConversationMessagesQuery : IQuery
{
    public Guid ConversationId { get; init; }

    public GetUserConversationMessagesQuery() { }

    public GetUserConversationMessagesQuery(Guid conversationId) => ConversationId = conversationId;
}