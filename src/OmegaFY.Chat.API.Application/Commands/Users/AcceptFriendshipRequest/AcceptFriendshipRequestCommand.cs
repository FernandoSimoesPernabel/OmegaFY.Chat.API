namespace OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;

public sealed record class AcceptFriendshipRequestCommand : ICommand
{
    public Guid FriendshipId { get; init; }

    public AcceptFriendshipRequestCommand() { }

    public AcceptFriendshipRequestCommand(Guid friendshipId) => FriendshipId = friendshipId;
}