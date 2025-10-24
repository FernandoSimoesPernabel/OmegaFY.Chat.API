namespace OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;

public sealed record class GetFriendshipByIdQuery : IQuery
{
    public Guid FriendshipId { get; init; }

    public GetFriendshipByIdQuery() { }

    public GetFriendshipByIdQuery(Guid friendshipId) => FriendshipId = friendshipId;
}