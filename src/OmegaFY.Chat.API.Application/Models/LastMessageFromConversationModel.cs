namespace OmegaFY.Chat.API.Application.Models;

public sealed record class LastMessageFromConversationModel
{
    public Guid MessageId { get; init; }

    public Guid ConversationId { get; init; }

    public Guid SenderMemberId { get; init; }

    public DateTime SentDate { get; init; }

    public string Content { get; init; }

    public string SenderDisplayName { get; init; }

    public LastMessageFromConversationModel() { }

    public LastMessageFromConversationModel(Guid messageId, Guid conversationId, Guid senderMemberId, DateTime sentDate, string content, string senderDisplayName)
    {
        MessageId = messageId;
        ConversationId = conversationId;
        SenderMemberId = senderMemberId;
        SentDate = sentDate;
        Content = content;
        SenderDisplayName = senderDisplayName;
    }
}