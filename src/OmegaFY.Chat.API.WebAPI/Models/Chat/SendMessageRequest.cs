using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.WebAPI.Models.Chat;

public sealed record class SendMessageRequest
{
    public MessageType Type { get; init; }

    public string Body { get; init; }

    public SendMessageCommand ToCommand(Guid conversationId) => new SendMessageCommand(conversationId, Type, Body);
}