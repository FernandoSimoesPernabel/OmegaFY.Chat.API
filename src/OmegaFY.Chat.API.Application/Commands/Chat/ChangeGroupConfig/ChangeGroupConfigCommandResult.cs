namespace OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

public sealed record class ChangeGroupConfigCommandResult : ICommandResult
{
    public Guid ConversationId { get; init; }

    public Guid CreatedByUserId { get; init; }

    public string GroupName { get; init; }

    public byte MaxNumberOfMembers { get; init; }

    public ChangeGroupConfigCommandResult() { }

    public ChangeGroupConfigCommandResult(Guid conversationId, Guid createdByUserId, string groupName, byte maxNumberOfMembers)
    {
        ConversationId = conversationId;
        CreatedByUserId = createdByUserId;
        GroupName = groupName;
        MaxNumberOfMembers = maxNumberOfMembers;
    }
}