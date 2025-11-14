namespace OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

public sealed record class ChangeGroupConfigCommand : ICommand
{
    public Guid ConversationId { get; init; }

    public string NewGroupName { get; init; }

    public byte NewMaxNumberOfMembers { get; init; }

    public ChangeGroupConfigCommand() { }

    public ChangeGroupConfigCommand(Guid conversationId, string newGroupName, byte newMaxNumberOfMembers)
    {
        ConversationId = conversationId;
        NewGroupName = newGroupName;
        NewMaxNumberOfMembers = newMaxNumberOfMembers;
    }
}