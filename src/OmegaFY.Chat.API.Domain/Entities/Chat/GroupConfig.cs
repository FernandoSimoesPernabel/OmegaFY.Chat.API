using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class GroupConfig : Entity
{
    public ReferenceId ConversationId { get; }

    public ReferenceId CreatedByUserId { get; }

    public string GroupName { get; private set; }

    public byte MaxNumberOfMembers { get; private set; }

    internal GroupConfig() { }

    public GroupConfig(ReferenceId conversationId, ReferenceId createdByUserId, string groupName, byte maxNumberOfMembers)
    {
        //TODO validar
        ConversationId = conversationId;
        CreatedByUserId = createdByUserId;
        ChangeGroupName(groupName);
        ChangeMaxNumberOfMembers(maxNumberOfMembers);
    }

    internal void ChangeGroupName(string groupName)
    {
        //TODO
        if (string.IsNullOrWhiteSpace(groupName))
            throw new DomainArgumentException("");

        if (groupName.Length > 100) //TODO 
            throw new DomainArgumentException("");

        GroupName = groupName;
    }

    internal void ChangeMaxNumberOfMembers(byte maxNumberOfMembers) => MaxNumberOfMembers = Math.Min(maxNumberOfMembers, (byte)100); //TODO constant
}