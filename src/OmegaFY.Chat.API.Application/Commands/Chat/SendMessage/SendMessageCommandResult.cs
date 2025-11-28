namespace OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;

public sealed record class SendMessageCommandResult : ICommandResult
{
    public Guid ConversationId { get; init; }

    public Guid MessageId { get; init; }

    public SendMessageCommandResult() { }

    public SendMessageCommandResult(Guid conversationId, Guid messageId)
    {
        ConversationId = conversationId;
        MessageId = messageId;
    }
}