using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class MemberAndMessageModel
{
    public Guid MessageId { get; init; }

    public Guid SenderMemberId { get; init; }

    public Guid DestinationMemberId { get; init; }

    public DateTime SendDate { get; init; }

    public DateTime DeliveryDate { get; init; }

    public MessageType Type { get; init; }

    public MemberMessageStatus Status { get; init; }

    public string Body { get; init; }

    public MemberAndMessageModel() { }

    public MemberAndMessageModel(
        Guid messageId,
        Guid senderMemberId,
        Guid destinationMemberId,
        DateTime sendDate,
        DateTime deliveryDate,
        MessageType type,
        MemberMessageStatus status,
        string body)
    {
        MessageId = messageId;
        SenderMemberId = senderMemberId;
        DestinationMemberId = destinationMemberId;
        SendDate = sendDate;
        DeliveryDate = deliveryDate;
        Type = type;
        Status = status;
        Body = body;
    }
}