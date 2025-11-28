using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class ConversationAndMembersModel
{
    public Guid ConversationId { get; init; }

    public ConversationType Type { get; init; }

    public ConversationStatus Status { get; init; }

    public DateTime CreatedDate { get; init; }

    public GroupConfigModel GroupConfig { get; init; }

    public MemberModel[] Members { get; init; }
}