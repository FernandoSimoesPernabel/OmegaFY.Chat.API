namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed record class AddMemberToGroupCommand : ICommand
{
    public Guid ConversationId { get; init; }

    public Guid UserId { get; init; }

    public AddMemberToGroupCommand() { }

    public AddMemberToGroupCommand(Guid conversationId, Guid userId)
    {
        ConversationId = conversationId;
        UserId = userId;
    }
}