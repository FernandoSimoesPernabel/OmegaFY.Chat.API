namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed record class AddMemberToGroupCommandResult : ICommandResult
{
    public Guid MemberId { get; init; }

    public Guid ConversationId { get; init; }

    public AddMemberToGroupCommandResult() { }

    public AddMemberToGroupCommandResult(Guid memberId, Guid conversationId)
    {
        MemberId = memberId;
        ConversationId = conversationId;
    }
}