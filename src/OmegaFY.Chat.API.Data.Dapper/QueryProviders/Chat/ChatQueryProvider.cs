using Dapper;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using System.Data;

namespace OmegaFY.Chat.API.Data.Dapper.QueryProviders.Chat;

internal sealed class ChatQueryProvider : IChatQueryProvider
{
    private readonly IDbConnection _dbConnection;

    public ChatQueryProvider(IDbConnection dbConnection) => _dbConnection = dbConnection;

    public async Task<ConversationAndMembersModel> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT TOP 1
                C.Id AS ConversationId,
                C.Type, 
                C.Status,
                C.CreatedDate,
                GC.Id AS GroupConfigId,
                GC.CreatedByUserId,
                GC.GroupName,
                GC.MaxNumberOfMembers
            
            FROM 
                chat.Conversations AS C

            LEFT JOIN
                chat.GroupConfigs AS GC ON C.Id = GC.ConversationId 
            
            WHERE 
                C.Id = @ConversationId;

            SELECT
                M.Id AS MemberId,
                M.ConversationId,
                M.UserId, 
                M.JoinedDate

            FROM
                chat.Members AS M

            WHERE
                M.ConversationId = @ConversationId";

        //https://github.com/DapperLib/Dapper/issues/452
        await using SqlMapper.GridReader gridReader = await _dbConnection.QueryMultipleAsync(sql, new { ConversationId = conversationId });

        ConversationAndMembersModel conversation = await gridReader.ReadFirstOrDefaultAsync<ConversationAndMembersModel>();

        //ConversationModel conversation2 = gridReader.Read<ConversationModel, GroupConfigModel, ConversationModel>(
        //    (conversation, groupConfig) => conversation with { GroupConfig = groupConfig },
        //    splitOn: nameof(GroupConfigModel.GroupConfigId)).FirstOrDefault();

        if (conversation is null)
            return null;

        return conversation with
        {
            Members = (await gridReader.ReadAsync<MemberModel>()).ToArray()
        };
    }

    public async Task<MemberModel> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
                M.Id AS MemberId,
                M.ConversationId,
                M.UserId, 
                M.JoinedDate

            FROM
                chat.Members AS M

            WHERE
                M.Id = @MemberId";

        return await _dbConnection.QueryFirstOrDefaultAsync<MemberModel>(sql, new { MemberId = memberId });
    }

    public async Task<MemberAndMessageModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT TOP 1
                Message.Id AS MessageId,
                Message.SenderMemberId,
                MemberMessage.DestinationMemberId,
                Message.SendDate,
                MemberMessage.DeliveryDate,
                Message.Type,
                MemberMessage.Status,
                Message.Body
            
            FROM
                chat.Messages AS Message
            
            INNER JOIN
                chat.MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id

            INNER JOIN
                chat.Members AS Member ON (Member.Id = MemberMessage.DestinationMemberId OR Member.Id = MemberMessage.SenderMemberId) AND Member.UserId = @UserId
            
            WHERE
                Message.Id = @MessageId";

        return await _dbConnection.QueryFirstOrDefaultAsync<MemberAndMessageModel>(sql, new { MessageId = messageId, UserId = userId });
    }
}