using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
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
        ConversationId = conversationId;
        CreatedByUserId = createdByUserId;
        ChangeGroupName(groupName);
        ChangeMaxNumberOfMembers(maxNumberOfMembers);
    }

    internal void ChangeGroupName(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new DomainArgumentException("O nome do grupo não foi informado.");

        if (groupName.Length > ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH)
            throw new DomainArgumentException($"O nome do grupo não pode exceder {ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH} caracteres.");

        GroupName = groupName;
    }

    internal void ChangeMaxNumberOfMembers(byte maxNumberOfMembers)
    {
        byte safeMaxNumberOfMembers = Math.Min(Math.Max(maxNumberOfMembers, ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS), ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS);
        MaxNumberOfMembers = safeMaxNumberOfMembers;
    }
}