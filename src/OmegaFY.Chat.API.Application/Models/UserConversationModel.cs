using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class UserConversationModel
{
    public Guid ConversationId { get; init; }

    public string DisplayName { get; init; }

    public ConversationType Type { get; init; }

    public ConversationStatus Status { get; init; }

    public LastMessageFromConversationModel LastMessage { get; init; }
}