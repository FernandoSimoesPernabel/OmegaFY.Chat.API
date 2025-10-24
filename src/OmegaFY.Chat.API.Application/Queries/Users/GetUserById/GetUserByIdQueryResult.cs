using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed record class GetUserByIdQueryResult : IQueryResult
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string DisplayName { get; init; }

    public FriendshipStatus? FriendshipStatus { get; init; }
}