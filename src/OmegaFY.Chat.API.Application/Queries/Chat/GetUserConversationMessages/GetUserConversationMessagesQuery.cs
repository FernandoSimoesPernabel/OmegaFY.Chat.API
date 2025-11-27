using OmegaFY.Chat.API.Common.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed record GetUserConversationMessagesQuery : IQuery
{
    public Guid ConversationId { get; init; }

    public Pagination Pagination { get; init; }

    public GetUserConversationMessagesQuery() { }

    public GetUserConversationMessagesQuery(Guid conversationId, Pagination pagination)
    {
        ConversationId = conversationId;
        Pagination = pagination;
    }
}