namespace OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;

public sealed record SendFriendshipRequestCommandResult : ICommandResult
{
    public Guid FriendshipId { get; init; }

    public SendFriendshipRequestCommandResult() { }

    public SendFriendshipRequestCommandResult(Guid friendshipId) => FriendshipId = friendshipId;
}