using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversations;

public sealed record GetUserConversationsQueryResult : IQueryResult
{
    public UserConversationModel[] UserConversations { get; init; } = [];

    public GetUserConversationsQueryResult() { }

    public GetUserConversationsQueryResult(UserConversationModel[] userConversations) => UserConversations = userConversations ?? [];
}