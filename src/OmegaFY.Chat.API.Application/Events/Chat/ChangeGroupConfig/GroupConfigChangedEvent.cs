namespace OmegaFY.Chat.API.Application.Events.Chat.ChangeGroupConfig;

public sealed record class GroupConfigChangedEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public string NewGroupName { get; init; }

    public byte NewMaxNumberOfMembers { get; init; }

    public GroupConfigChangedEvent() { }

    public GroupConfigChangedEvent(Guid conversationId, string newGroupName, byte newMaxNumberOfMembers)
    {
        ConversationId = conversationId;
        NewGroupName = newGroupName;
        NewMaxNumberOfMembers = newMaxNumberOfMembers;
    }
}