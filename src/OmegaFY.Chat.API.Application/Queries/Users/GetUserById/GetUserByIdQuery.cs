namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed record class GetUserByIdQuery : IQuery
{
    public Guid UserId { get; init; }

    public GetUserByIdQuery() { }

    public GetUserByIdQuery(Guid userId) => UserId = userId;
}