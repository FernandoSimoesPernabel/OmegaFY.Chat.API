using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class Conversation : Entity, IAggregateRoot<Conversation>
{
    private readonly List<Member> _members = new List<Member>();

    public ConversationType Type { get; }

    public ConversationStatus Status { get; private set; }

    public DateTime CreatedDate { get; }

    public GroupConfig GroupConfig { get; }

    public IReadOnlyCollection<Member> Members => _members.AsReadOnly();

    internal Conversation() { }

    private Conversation(GroupConfig groupConfig) : this(ConversationType.GroupChat, groupConfig) { }

    private Conversation(ReferenceId memberOneUserId, ReferenceId memberTwoUserId) : this(ConversationType.MemberToMember, null)
    {
        if (memberOneUserId == memberTwoUserId)
            throw new DomainArgumentException("Não é possível criar uma conversa entre o mesmo usuário.");

        _members.Add(new Member(Id, memberOneUserId));
        _members.Add(new Member(Id, memberTwoUserId));
    }

    private Conversation(ConversationType conversationType, GroupConfig groupConfig)
    {
        if (conversationType == ConversationType.GroupChat && groupConfig is null)
            throw new DomainArgumentException("Não é possível criar uma conversa em grupo sem configuração de grupo.");

        Type = conversationType;
        GroupConfig = conversationType == ConversationType.GroupChat ? groupConfig : null;

        Status = ConversationStatus.Open;
        CreatedDate = DateTime.UtcNow;
    }

    public void AddMemberToGroup(ReferenceId userIdToAdd)
    {
        if (Type != ConversationType.GroupChat)
            throw new DomainInvalidOperationException("Não é possível adicionar membros em uma conversa que não é em grupo.");

        if (IsMemberInConversation(userIdToAdd))
            throw new DomainInvalidOperationException("Usuário já é membro da conversa.");

        _members.Add(new Member(Id, userIdToAdd));
    }

    public void RemoveMemberFromGroup(ReferenceId userIdToRemove)
    {
        if (Type != ConversationType.GroupChat)
            throw new DomainInvalidOperationException("Não é possível remover membros em uma conversa que não é em grupo.");

        _members.RemoveAll(member => member.UserId == userIdToRemove);
    }

    public void ChangeGroupConfig(string newGroupName, byte newMaxNumberOfMembers)
    {
        if (Type != ConversationType.GroupChat)
            throw new DomainInvalidOperationException("Não é possível alterar a configuração de uma conversa que não é em grupo.");

        if (_members.Count > newMaxNumberOfMembers)
            throw new DomainArgumentException("O número máximo de membros não pode ser menor que o número atual de membros.");

        GroupConfig.ChangeGroupName(newGroupName);
        GroupConfig.ChangeMaxNumberOfMembers(newMaxNumberOfMembers);
    }

    public bool IsMemberInConversation(ReferenceId userId) => _members.Exists(member => member.UserId == userId);

    public Member GetMemberByUserId(ReferenceId userId) => _members.Find(member => member.UserId == userId);

    public static Conversation StartMemberToMemberConversation(ReferenceId memberOneUserId, ReferenceId memberTwoUserId) => new Conversation(memberOneUserId, memberTwoUserId);

    public static Conversation CreateGroupChat(GroupConfig groupConfig) => new Conversation(groupConfig);
}