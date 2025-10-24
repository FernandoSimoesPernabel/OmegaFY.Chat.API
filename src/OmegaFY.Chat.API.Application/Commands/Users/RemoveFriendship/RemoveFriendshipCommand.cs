namespace OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;

public sealed record class RemoveFriendshipCommand : ICommand
{
    public Guid FriendshipId { get; init; }

    public RemoveFriendshipCommand() { }

    public RemoveFriendshipCommand(Guid friendshipId) => FriendshipId = friendshipId;
}