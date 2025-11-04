namespace OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;

public sealed record class CreateGroupConversationCommandResult : ICommandResult
{
    public Guid ConversationId { get; init; }

    public CreateGroupConversationCommandResult() { }

    public CreateGroupConversationCommandResult(Guid conversationId) => ConversationId = conversationId;
}