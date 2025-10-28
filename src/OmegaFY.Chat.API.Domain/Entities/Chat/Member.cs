using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Chat;

public sealed class Member : Entity
{
    public ReferenceId ConversationId { get; }

    public ReferenceId UserId { get; }

    public DateTime JoinedDate { get; }

    internal Member() { }

    public Member(ReferenceId conversationId, ReferenceId userId)
    {
        //TODO
        ConversationId = conversationId;
        UserId = userId;

        JoinedDate = DateTime.UtcNow;
    }
}