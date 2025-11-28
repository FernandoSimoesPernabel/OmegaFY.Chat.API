using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class MemberMessage : Entity, IAggregateRoot<MemberMessage>
{
    public ReferenceId MessageId { get; }

    public ReferenceId SenderMemberId { get; }

    public ReferenceId DestinationMemberId { get; }

    public DateTime DeliveryDate { get; }

    public MemberMessageStatus Status { get; private set; }

    internal MemberMessage() { }

    public MemberMessage(ReferenceId messageId, ReferenceId senderMemberId, ReferenceId destinationMemberId)
    {
        MessageId = messageId;
        SenderMemberId = senderMemberId;
        DestinationMemberId = destinationMemberId;
        Status = MemberMessageStatus.Unread;

        DeliveryDate = DateTime.UtcNow;
    }

    public void Read()
    {
        if (IsUnread())
            Status = MemberMessageStatus.Read;
    }

    public void Delete() => Status = MemberMessageStatus.Deleted;

    public bool IsRead() => Status == MemberMessageStatus.Read;

    public bool IsUnread() => Status == MemberMessageStatus.Unread;

    public bool IsDeleted() => Status == MemberMessageStatus.Deleted;
}