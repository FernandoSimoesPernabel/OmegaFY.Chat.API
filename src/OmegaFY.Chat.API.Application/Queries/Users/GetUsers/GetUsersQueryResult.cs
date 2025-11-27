using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUsers;

public sealed record GetUsersQueryResult : IQueryResult
{
    public UserModel[] Users { get; init; } = [];

    public GetUsersQueryResult() { }

    public GetUsersQueryResult(UserModel[] users) => Users = users ?? [];
}