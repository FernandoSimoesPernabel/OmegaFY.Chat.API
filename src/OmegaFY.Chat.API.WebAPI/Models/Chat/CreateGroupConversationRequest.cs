using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;

namespace OmegaFY.Chat.API.WebAPI.Models.Chat;

public sealed record class CreateGroupConversationRequest
{
    public string GroupName { get; init; }

    public byte MaxNumberOfMembers { get; init; }

    public CreateGroupConversationCommand ToCommand() => new CreateGroupConversationCommand(GroupName, MaxNumberOfMembers);
}