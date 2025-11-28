namespace OmegaFY.Chat.API.Application.Models;

public sealed record class MemberModel
{
    public Guid MemberId { get; init; }

    public Guid ConversationId { get; init; }

    public Guid UserId { get; init; }

    public DateTime JoinedDate { get; init; }
}