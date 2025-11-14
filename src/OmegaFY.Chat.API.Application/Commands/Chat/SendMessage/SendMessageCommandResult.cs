namespace OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;

public sealed record class SendMessageCommandResult : ICommandResult
{
    public Guid MessageId { get; init; }

    public SendMessageCommandResult() { }

    public SendMessageCommandResult(Guid messageId) => MessageId = messageId;
}