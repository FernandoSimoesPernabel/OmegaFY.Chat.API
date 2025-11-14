namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;

public sealed record MarkMessageAsReadCommand : ICommand
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public MarkMessageAsReadCommand() { }

    public MarkMessageAsReadCommand(Guid conversationId, Guid messageId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
    }
}