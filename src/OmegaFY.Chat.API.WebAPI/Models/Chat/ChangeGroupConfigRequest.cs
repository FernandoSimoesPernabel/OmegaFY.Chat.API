using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

namespace OmegaFY.Chat.API.WebAPI.Models.Chat;

public sealed record class ChangeGroupConfigRequest
{
    public string NewGroupName { get; init; }

    public byte NewMaxNumberOfMembers { get; init; }

    public ChangeGroupConfigCommand ToCommand(Guid conversationId) => new ChangeGroupConfigCommand(conversationId, NewGroupName, NewMaxNumberOfMembers);
}