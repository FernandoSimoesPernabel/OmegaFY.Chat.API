using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class UserModel
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string DisplayName { get; init; }

    public FriendshipStatus? FriendshipStatus { get; init; }
}
