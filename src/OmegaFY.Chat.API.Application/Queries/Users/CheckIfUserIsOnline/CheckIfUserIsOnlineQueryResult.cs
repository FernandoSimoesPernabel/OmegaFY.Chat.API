namespace OmegaFY.Chat.API.Application.Queries.Users.CheckIfUserIsOnline;

public sealed record class CheckIfUserIsOnlineQueryResult : IQueryResult
{
    public bool IsOnline { get; init; }

    public CheckIfUserIsOnlineQueryResult() { }

    public CheckIfUserIsOnlineQueryResult(bool isOnline) => IsOnline = isOnline;
}