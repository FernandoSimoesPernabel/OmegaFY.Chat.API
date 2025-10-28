using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class GroupConfig : Entity
{
    public ReferenceId ConversationId { get; }

    public ReferenceId CreatedByUserId { get; }

    public string GroupName { get; }

    public byte MaxNumberOfMembers { get; }

    internal GroupConfig() { }

    public GroupConfig(ReferenceId conversationId, ReferenceId createdByUserId, string groupName, byte maxNumberOfMembers)
    {
        //TODO
        if (string.IsNullOrWhiteSpace(groupName))
            throw new DomainArgumentException("");

        if (groupName.Length > 100) //TODO 
            throw new DomainArgumentException("");

        ConversationId = conversationId;
        CreatedByUserId = createdByUserId;
        GroupName = groupName;
        MaxNumberOfMembers = Math.Min(maxNumberOfMembers, (byte)100);
    }
}