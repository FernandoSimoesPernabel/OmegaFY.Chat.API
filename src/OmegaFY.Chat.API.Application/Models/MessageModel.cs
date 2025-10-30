using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class MessageModel
{
    public Guid MessageId { get; init; }

    public Guid SenderMemberId { get; init; }

    public DateTime SendDate { get; init; }

    public MessageType Type { get; init; }

    public string Body { get; init; }

    public MessageModel() { }

    public MessageModel(Guid messageId, Guid senderMemberId, DateTime sendDate, MessageType type, string body)
    {
        MessageId = messageId;
        SenderMemberId = senderMemberId;
        SendDate = sendDate;
        Type = type;
        Body = body;
    }
}