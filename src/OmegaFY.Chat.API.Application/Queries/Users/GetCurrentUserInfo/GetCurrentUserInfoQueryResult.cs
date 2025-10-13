using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

public sealed record class GetCurrentUserInfoQueryResult : IQueryResult
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string DisplayName { get; init; }

    public FriendshipModel[] Friendships { get; init; } = [];
}