namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed record class AddMemberToGroupCommandResult : ICommandResult
{
    public Guid MemberId { get; init; }

    public AddMemberToGroupCommandResult() { }

    public AddMemberToGroupCommandResult(Guid memberId) => MemberId = memberId;
}