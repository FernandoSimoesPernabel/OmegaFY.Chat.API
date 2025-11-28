using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed record class GetConversationByIdQueryResult : IQueryResult
{
    public ConversationAndMembersModel Conversation { get; init; }

    public GetConversationByIdQueryResult() { }

    public GetConversationByIdQueryResult(ConversationAndMembersModel conversation)
    {
        Conversation = conversation;
    }
}