using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;

public sealed record class GetFriendshipByIdQueryResult : IQueryResult
{
    public FriendshipModel Friendship { get; init; }

    public GetFriendshipByIdQueryResult() { }

    public GetFriendshipByIdQueryResult(FriendshipModel friendship) => Friendship = friendship;
}