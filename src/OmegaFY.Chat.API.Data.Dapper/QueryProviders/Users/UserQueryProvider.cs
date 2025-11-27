using Dapper;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Application.Queries.Users.GetUserById;
using System.Data;

namespace OmegaFY.Chat.API.Data.Dapper.QueryProviders.Users;

internal sealed class UserQueryProvider : IUserQueryProvider
{
    private readonly IDbConnection _dbConnection;

    public UserQueryProvider(IDbConnection dbConnection) => _dbConnection = dbConnection;

    public async Task<GetCurrentUserInfoQueryResult> GetCurrentUserInfoAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @"
            SELECT TOP 1
                U.Id, 
                U.DisplayName, 
                U.Email
            
            FROM 
                chat.Users AS U
            
            WHERE 
                U.Id = @UserId;

            SELECT
                F.Id AS FriendshipId,
                F.RequestingUserId,
                F.InvitedUserId,
                F.StartedDate,
                F.Status
            
            FROM 
                chat.Friendships AS F
            
            WHERE 
                (F.RequestingUserId = @UserId OR F.InvitedUserId = @UserId)";

        await using SqlMapper.GridReader gridReader = await _dbConnection.QueryMultipleAsync(sql, new { UserId = userId });

        GetCurrentUserInfoQueryResult result = await gridReader.ReadFirstOrDefaultAsync<GetCurrentUserInfoQueryResult>();

        if (result is null)
            return null;

        return result with
        {
            Friendships = (await gridReader.ReadAsync<FriendshipModel>()).ToArray()
        };
    }

    public Task<FriendshipModel> GetFriendshipByIdAndUserIdAsync(Guid userId, Guid friendshipId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @"
            SELECT TOP 1
                Id AS FriendshipId, 
                RequestingUserId, 
                InvitedUserId,
                StartedDate,
                Status
            
            FROM 
                chat.Friendships
            
            WHERE 
                Id = @FriendshipId AND (RequestingUserId = @UserId OR InvitedUserId = @UserId)";

        return _dbConnection.QueryFirstOrDefaultAsync<FriendshipModel>(sql, new { FriendshipId = friendshipId, UserId = userId });
    }

    public Task<GetUserByIdQueryResult> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @"
            SELECT TOP 1
                U.Id, 
                U.DisplayName, 
                U.Email,
                F.Status AS FriendshipStatus
            
            FROM 
                chat.Users AS U

            LEFT JOIN 
                chat.Friendships AS F ON (F.InvitedUserId = U.Id) OR (F.RequestingUserId = U.Id)
            
            WHERE 
                U.Id = @UserId;";

        return _dbConnection.QueryFirstOrDefaultAsync<GetUserByIdQueryResult>(sql, new { UserId = userId });
    }
}