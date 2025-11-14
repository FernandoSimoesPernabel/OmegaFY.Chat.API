namespace OmegaFY.Chat.API.Application.Events.Chat.CreateGroupConversation;

public sealed record class GroupConversationCreatedEvent : IEvent
{
    public Guid ConversationId { get; init; }

    public GroupConversationCreatedEvent() { }

    public GroupConversationCreatedEvent(Guid conversationId) => ConversationId = conversationId;
}