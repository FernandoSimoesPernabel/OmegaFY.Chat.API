using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;

public interface IUserQueryProvider : IQueryProvider
{
    public Task<GetCurrentUserInfoQueryResult> GetCurrentUserInfoAsync(Guid userId, CancellationToken cancellationToken);
}