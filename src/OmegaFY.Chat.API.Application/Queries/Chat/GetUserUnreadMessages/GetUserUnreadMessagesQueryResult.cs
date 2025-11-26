using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;

public sealed record GetUserUnreadMessagesQueryResult : IQueryResult
{
    public MessageModel[] Messages { get; init; } = [];

    public GetUserUnreadMessagesQueryResult() { }

    public GetUserUnreadMessagesQueryResult(MessageModel[] messages) => Messages = messages;
}