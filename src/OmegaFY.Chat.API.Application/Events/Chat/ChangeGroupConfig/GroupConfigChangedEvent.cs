namespace OmegaFY.Chat.API.Application.Events.Chat.ChangeGroupConfig;

public sealed record class GroupConfigChangedEvent(Guid ConversationId) : IEvent;