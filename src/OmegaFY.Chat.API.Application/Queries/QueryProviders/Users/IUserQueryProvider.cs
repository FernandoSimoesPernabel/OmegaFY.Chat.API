using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;

public interface IUserQueryProvider : IQueryProvider
{
    public Task<GetCurrentUserInfoQueryResult> GetCurrentUserInfoAsync(Guid userId, CancellationToken cancellationToken);

    public Task<FriendshipModel> GetFriendshipByIdAsync(Guid userId, Guid friendshipId, CancellationToken cancellationToken);
    
    public Task<GetUserByIdQueryResult> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
}