namespace OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;

public sealed record class RemoveMemberFromGroupCommand : ICommand
{
    public Guid ConversationId { get; init; }

    public Guid MemberId { get; init; }

    public RemoveMemberFromGroupCommand() { }

    public RemoveMemberFromGroupCommand(Guid conversationId, Guid memberId)
    {
        ConversationId = conversationId;
        MemberId = memberId;
    }
}