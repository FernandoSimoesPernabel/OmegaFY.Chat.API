using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed record GetUserConversationMessagesQueryResult : IQueryResult
{
    public MessageFromMemberModel[] Messages { get; init; } = [];

    public GetUserConversationMessagesQueryResult() { }

    public GetUserConversationMessagesQueryResult(MessageFromMemberModel[] messages) => Messages = messages ?? [];
}