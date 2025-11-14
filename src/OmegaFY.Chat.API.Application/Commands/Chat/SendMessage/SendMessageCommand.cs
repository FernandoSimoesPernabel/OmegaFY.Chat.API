using OmegaFY.Chat.API.Domain.Enums;
namespace OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;

public sealed record class SendMessageCommand : ICommand
{
    public Guid ConversationId { get; init; }

    public MessageType Type { get; init; }

    public string Body { get; init; }

    public SendMessageCommand() { }

    public SendMessageCommand(Guid conversationId, MessageType type, string body)
    {
        ConversationId = conversationId;
        Type = type;
        Body = body;
    }
}