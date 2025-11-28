using OmegaFY.Chat.API.Application.Queries.Users.GetUsers;
using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.WebAPI.Models.Users;

public class GetUsersRequest
{
    public string DisplayName { get; init; }

    public FriendshipStatus? Status { get; init; }

    public GetUsersQuery ToQuery() => new GetUsersQuery(DisplayName, Status);
}
