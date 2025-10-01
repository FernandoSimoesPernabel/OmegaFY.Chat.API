namespace OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

public sealed record class GetCurrentUserInfoQueryResult : IQueryResult
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string DisplayName { get; set; }
}