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
        _members.Add(new Member(Id, memberOneUserId));
        _members.Add(new Member(Id, memberTwoUserId));
    }

    private Conversation(ConversationType conversationType, GroupConfig groupConfig)
    {
        //TODO
        if (conversationType == ConversationType.GroupChat && groupConfig is null)
            throw new DomainArgumentException("");

        Type = conversationType;
        GroupConfig = conversationType == ConversationType.GroupChat ? groupConfig : null;

        Status = ConversationStatus.Open;
        CreatedDate = DateTime.UtcNow;
    }

    public static Conversation StartMemberToMemberConversation(ReferenceId memberOneUserId, ReferenceId memberTwoUserId) => new Conversation(memberOneUserId, memberTwoUserId);

    public static Conversation CreateGroupChat(GroupConfig groupConfig) => new Conversation(groupConfig);
}