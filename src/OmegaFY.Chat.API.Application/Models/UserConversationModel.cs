namespace OmegaFY.Chat.API.Application.Models;

public sealed record class UserConversationModel
{
    public Guid ConversationId { get; init; }

    public string DisplayName { get; init; }

    public LastMessageFromConversationModel LastMessage { get; init; }

    public UserConversationModel() { }

    public UserConversationModel(Guid conversationId, string displayName, LastMessageFromConversationModel lastMessage)
    {
        ConversationId = conversationId;
        DisplayName = displayName;
        LastMessage = lastMessage;
    }
}