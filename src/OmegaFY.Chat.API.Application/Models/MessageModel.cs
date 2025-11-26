using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class MessageModel
{
    public Guid MessageId { get; init; }

    public Guid ConversationId { get; init; }

    public Guid SenderMemberId { get; init; }

    public DateTime SendDate { get; init; }

    public MessageType Type { get; init; }

    public string Content { get; init; }
}