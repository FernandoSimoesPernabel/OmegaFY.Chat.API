namespace OmegaFY.Chat.API.Application.Events.Chat.AddMemberToGroup;

public sealed record class MemberAddedToGroupEvent(Guid ConversationId, Guid MemberId) : IEvent;