using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class MemberMessage : Entity
{
    public ReferenceId MessageId { get; }

    public ReferenceId SenderMemberId { get; }

    public ReferenceId DestinationMemberId { get; }

    public DateTime DeliveryDate { get; }

    public MemberMessageStatus Status { get; private set; }

    internal MemberMessage() { }

    public MemberMessage(ReferenceId messageId, ReferenceId senderMemberId, ReferenceId destinationMemberId, MemberMessageStatus messageStatus)
    {
        if (!messageStatus.IsDefined())
            throw new DomainArgumentException($"O status não é válido para a mensagem do membro.");

        MessageId = messageId;
        SenderMemberId = senderMemberId;
        DestinationMemberId = destinationMemberId;
        Status = messageStatus;

        DeliveryDate = DateTime.UtcNow;
    }
}