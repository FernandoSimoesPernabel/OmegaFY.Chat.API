namespace OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;

public sealed record class SendFriendshipRequestCommand : ICommand
{
    public Guid InvitedUserId { get; init; }

    public SendFriendshipRequestCommand() { }

    public SendFriendshipRequestCommand(Guid invitedUserId) => InvitedUserId = invitedUserId;
}