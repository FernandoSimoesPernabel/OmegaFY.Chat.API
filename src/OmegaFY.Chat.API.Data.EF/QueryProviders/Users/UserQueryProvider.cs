using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Domain.Entities.Users;

namespace OmegaFY.Chat.API.Data.EF.QueryProviders.Users;

internal sealed class UserQueryProvider : IUserQueryProvider
{
    private readonly ApplicationContext _context;

    public UserQueryProvider(ApplicationContext context) => _context = context;

    public async Task<GetCurrentUserInfoQueryResult> GetCurrentUserInfoAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<GetCurrentUserInfoQueryResult>()
            .FromSqlInterpolated($"SELECT Id, DisplayName, Email FROM chat.Users WHERE Id = {userId}")
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<FriendshipModel> GetFriendshipByIdAsync(Guid userId, Guid friendshipId, CancellationToken cancellationToken)
    {
        return await _context.Set<FriendshipModel>()
            .FromSqlInterpolated(@$"
                SELECT TOP 1
                    Id AS FriendshipId, 
                    RequestingUserId, 
                    InvitedUserId,
                    StartedDate,
                    Status
                
                FROM 
                    chat.Friendship 
                
                WHERE 
                    Id = {friendshipId} AND (RequestingUserId = {userId} OR InvitedUserId = {userId})")
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}