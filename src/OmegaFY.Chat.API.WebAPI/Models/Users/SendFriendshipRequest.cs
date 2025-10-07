using OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;

namespace OmegaFY.Chat.API.WebAPI.Models.Users;

public sealed record class SendFriendshipRequest
{
    public Guid InvitedUserId { get; init; }

    public SendFriendshipRequestCommand ToCommand() => new SendFriendshipRequestCommand(InvitedUserId);
}