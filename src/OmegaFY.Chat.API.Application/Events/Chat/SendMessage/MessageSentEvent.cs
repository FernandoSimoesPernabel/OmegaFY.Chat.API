namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

public sealed record class MessageSentEvent : IEvent
{
    public Guid MessageId { get; init; }

    public MessageSentEvent() { }

    public MessageSentEvent(Guid messageId) => MessageId = messageId;
}