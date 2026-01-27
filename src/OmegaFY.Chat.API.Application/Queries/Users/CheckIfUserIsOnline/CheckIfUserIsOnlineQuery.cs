namespace OmegaFY.Chat.API.Application.Queries.Users.CheckIfUserIsOnline;

public sealed record class CheckIfUserIsOnlineQuery : IQuery
{
    public Guid UserId { get; init; }

    public CheckIfUserIsOnlineQuery() { }

    public CheckIfUserIsOnlineQuery(Guid userId) => UserId = userId;
}