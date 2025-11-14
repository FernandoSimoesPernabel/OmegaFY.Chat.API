using OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

namespace OmegaFY.Chat.API.WebAPI.Models.Chat;

public sealed class AddMemberToGroupRequest
{
    public Guid UserId { get; init; }

    public AddMemberToGroupCommand ToCommand(Guid conversationId) => new AddMemberToGroupCommand(conversationId,  UserId);
}