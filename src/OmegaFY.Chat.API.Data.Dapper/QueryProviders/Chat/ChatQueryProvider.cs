using Dapper;
using Microsoft.VisualBasic;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Domain.Entities.Chat;
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
                GC.ConversationId,
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

        await using SqlMapper.GridReader gridReader = await _dbConnection.QueryMultipleAsync(sql, new { ConversationId = conversationId });

        ConversationAndMembersModel conversation = gridReader.Read<ConversationAndMembersModel, GroupConfigModel, ConversationAndMembersModel>(
            (conversation, groupConfig) => conversation with { GroupConfig = groupConfig },
            splitOn: nameof(GroupConfigModel.GroupConfigId)).FirstOrDefault();

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
            SELECT TOP 1
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

    public async Task<MessageFromMemberModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken)
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
                Message.Content
            
            FROM
                chat.Messages AS Message
            
            INNER JOIN
                chat.MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id

            INNER JOIN
                chat.Members AS Member ON Member.Id = MemberMessage.DestinationMemberId AND Member.UserId = @UserId
            
            WHERE
                Message.Id = @MessageId";

        return await _dbConnection.QueryFirstOrDefaultAsync<MessageFromMemberModel>(sql, new { MessageId = messageId, UserId = userId });
    }

    public async Task<UserConversationModel[]> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
	            Conversation.Id AS ConversationId,
	            ISNULL(Config.GroupName, [User].DisplayName) AS DisplayName,
	            Message.MessageId,
	            Message.ConversationId,
	            Message.SenderMemberId,
	            Message.SendDate,
	            Message.Content,
	            Message.SenderDisplayName
	
            FROM 
	            chat.Conversations AS Conversation

            INNER JOIN
	            chat.Members AS Member ON Member.ConversationId = Conversation.Id

            INNER JOIN
	            chat.Users AS [User] ON [User].Id = Member.UserId

            LEFT JOIN
	            chat.GroupConfigs AS Config ON Config.ConversationId = Conversation.Id

            LEFT JOIN
	            (
		            SELECT TOP 1 
			            Message.Id AS MessageId, 
			            Message.ConversationId,
			            Message.SenderMemberId,
			            Message.SendDate,
			            Message.Content,
			            [User].DisplayName AS SenderDisplayName

		            FROM 
			            chat.[Messages] AS Message

		            INNER JOIN
			            chat.Members AS Member ON Member.Id = Message.SenderMemberId

		            INNER JOIN
			            chat.Users AS [User] ON [User].Id = Member.UserId

		            WHERE
			            Member.UserId = @UserId

		            ORDER BY 
			            Message.SendDate DESC
	            ) AS Message ON Message.ConversationId = Conversation.Id

            WHERE
	            Member.UserId = @UserId";

        IEnumerable<UserConversationModel> userConversations = await _dbConnection.QueryAsync<UserConversationModel, LastMessageFromConversationModel, UserConversationModel>(
            sql, (userConversation, lastMessage) => userConversation with { LastMessage = lastMessage },
            new { UserId = userId },
            splitOn: nameof(LastMessageFromConversationModel.MessageId));

        return userConversations.ToArray();
    }
}