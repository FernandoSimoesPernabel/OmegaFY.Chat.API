namespace OmegaFY.Chat.API.Application.Events.Chat.RemoveMemberFromGroup;

public sealed record class MemberRemovedFromGroupEvent(Guid ConversationId, Guid MemberId) : IEvent;