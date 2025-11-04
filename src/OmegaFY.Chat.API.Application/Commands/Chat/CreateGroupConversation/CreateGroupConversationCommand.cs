namespace OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;

public sealed record class CreateGroupConversationCommand : ICommand
{
    public string GroupName { get; init; }

    public byte MaxNumberOfMembers { get; init; }

    public CreateGroupConversationCommand() { }

    public CreateGroupConversationCommand(string groupName, byte maxNumberOfMembers)
    {
        GroupName = groupName;
        MaxNumberOfMembers = maxNumberOfMembers;
    }
}