namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;

public sealed record MarkMessageAsDeletedCommand : ICommand
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public MarkMessageAsDeletedCommand() { }

    public MarkMessageAsDeletedCommand(Guid conversationId, Guid messageId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
    }
}