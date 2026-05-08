using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Common.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed record GetUserConversationMessagesQueryResult : IQueryResult
{
    public MessageFromMemberModel[] Messages { get; init; } = [];

    public CursorPaginationResultInfo<DateTime> PaginationInfo { get; init; }

    public GetUserConversationMessagesQueryResult() { }

    public GetUserConversationMessagesQueryResult(MessageFromMemberModel[] messages, CursorPaginationResultInfo<DateTime> paginationInfo)
    {
        Messages = messages ?? [];
        PaginationInfo = paginationInfo;
    }
}