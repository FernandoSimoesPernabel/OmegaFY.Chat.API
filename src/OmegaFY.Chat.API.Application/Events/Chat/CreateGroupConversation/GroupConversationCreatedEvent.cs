namespace OmegaFY.Chat.API.Application.Events.Chat.CreateGroupConversation;

public sealed record class GroupConversationCreatedEvent(Guid ConversationId) : IEvent;