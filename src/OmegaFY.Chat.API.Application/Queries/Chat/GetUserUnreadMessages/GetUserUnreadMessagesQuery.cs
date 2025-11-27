using OmegaFY.Chat.API.Common.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;

public sealed record GetUserUnreadMessagesQuery : IQuery
{
    public Pagination Pagination { get; init; }

    public GetUserUnreadMessagesQuery() { }

    public GetUserUnreadMessagesQuery(Pagination pagination) => Pagination = pagination;
}