using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed record class GetUserByIdQueryResult : IQueryResult
{
    public UserModel User { get; init; }

    public GetUserByIdQueryResult() { }

    public GetUserByIdQueryResult(UserModel user) => User = user;
}