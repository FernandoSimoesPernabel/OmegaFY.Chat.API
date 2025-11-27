using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Common.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;

public sealed record GetUserUnreadMessagesQueryResult : IQueryResult
{
    public MessageModel[] Messages { get; init; } = [];

    public PaginationResultInfo PaginationInfo { get; init; }

    public GetUserUnreadMessagesQueryResult() { }

    public GetUserUnreadMessagesQueryResult(MessageModel[] messages, PaginationResultInfo paginationInfo)
    {
        Messages = messages ?? [];
        PaginationInfo = paginationInfo;
    }
}