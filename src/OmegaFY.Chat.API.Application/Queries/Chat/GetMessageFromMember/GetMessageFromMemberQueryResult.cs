using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed record class GetMessageFromMemberQueryResult : IQueryResult
{
    public MessageFromMemberModel Message { get; init; }

    public GetMessageFromMemberQueryResult() { }

    public GetMessageFromMemberQueryResult(MessageFromMemberModel message)
    {
        Message = message;
    }
}