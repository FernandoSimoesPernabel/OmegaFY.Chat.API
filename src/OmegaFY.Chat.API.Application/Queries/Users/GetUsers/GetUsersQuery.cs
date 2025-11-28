using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUsers;

public sealed record GetUsersQuery : IQuery
{
    public string DisplayName { get; init; }

    public FriendshipStatus? Status { get; init; }

    public GetUsersQuery() { }

    public GetUsersQuery(string displayName, FriendshipStatus? status)
    {
        DisplayName = displayName;
        Status = status;
    }
}