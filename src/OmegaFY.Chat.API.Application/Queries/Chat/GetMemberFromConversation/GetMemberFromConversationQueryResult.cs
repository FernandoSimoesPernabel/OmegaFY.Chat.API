using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;

public sealed record class GetMemberFromConversationQueryResult : IQueryResult
{
    public MemberModel Member { get; init; }
}