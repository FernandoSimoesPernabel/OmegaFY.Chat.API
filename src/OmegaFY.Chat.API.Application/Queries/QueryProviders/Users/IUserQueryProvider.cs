using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;

public interface IUserQueryProvider : IQueryProvider
{
    public Task<GetCurrentUserInfoQueryResult> GetCurrentUserInfoAsync(Guid userId, CancellationToken cancellationToken);

    public Task<FriendshipModel> GetFriendshipByIdAndUserIdAsync(Guid userId, Guid friendshipId, CancellationToken cancellationToken);

    public Task<UserModel> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    public Task<UserModel[]> GetUsersAsync(Guid userId, string displayName, FriendshipStatus? status, CancellationToken cancellationToken);
}