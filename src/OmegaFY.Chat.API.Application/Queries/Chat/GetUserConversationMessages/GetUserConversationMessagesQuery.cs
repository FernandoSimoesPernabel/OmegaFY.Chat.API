using OmegaFY.Chat.API.Common.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed record GetUserConversationMessagesQuery : IQuery
{
    public Guid ConversationId { get; init; }

    public CursorPagination<DateTime> Pagination { get; init; }

    public GetUserConversationMessagesQuery() { }

    public GetUserConversationMessagesQuery(Guid conversationId, CursorPagination<DateTime> pagination)
    {
        ConversationId = conversationId;
        Pagination = pagination;
    }
}