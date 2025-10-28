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

    public MemberMessage(ReferenceId messageId, ReferenceId senderMemberId, ReferenceId destinationMemberId, MemberMessageStatus messageStatus)
    {
        //TODO
        MessageId = messageId;
        SenderMemberId = senderMemberId;
        DestinationMemberId = destinationMemberId;
        Status = messageStatus;

        DeliveryDate = DateTime.UtcNow;
    }
}