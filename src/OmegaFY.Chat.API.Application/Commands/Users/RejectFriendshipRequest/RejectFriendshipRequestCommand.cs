namespace OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;

public sealed record class RejectFriendshipRequestCommand : ICommand
{
    public Guid FriendshipId { get; init; }

    public RejectFriendshipRequestCommand() { }

    public RejectFriendshipRequestCommand(Guid friendshipId) => FriendshipId = friendshipId;
}