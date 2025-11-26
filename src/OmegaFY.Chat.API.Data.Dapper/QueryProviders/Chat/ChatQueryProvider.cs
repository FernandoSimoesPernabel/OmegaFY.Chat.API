using Dapper;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Domain.Enums;
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
                Message.ConversationId,
                DestinationMember.Id AS MemberId,
                Message.SenderMemberId,
                Sender.DisplayName AS SenderDisplayName,
                MemberMessage.DestinationMemberId,
                Destination.DisplayName AS DestinationDisplayName,
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
                chat.Members AS DestinationMember ON DestinationMember.Id = MemberMessage.DestinationMemberId AND DestinationMember.UserId = @UserId

            INNER JOIN
                chat.Members AS SenderMember ON SenderMember.Id = MemberMessage.SenderMemberId

            INNER JOIN
                chat.Users AS Destination ON Destination.Id = DestinationMember.UserId

            INNER JOIN
                chat.Users AS Sender ON Sender.Id = SenderMember.UserId

            WHERE
                Message.Id = @MessageId";

        return await _dbConnection.QueryFirstOrDefaultAsync<MessageFromMemberModel>(sql, new { MessageId = messageId, UserId = userId });
    }

    public async Task<MessageFromMemberModel[]> GetMessagesFromMemberAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
                Message.Id AS MessageId,
                Message.ConversationId,
                DestinationMember.Id AS MemberId,
                Message.SenderMemberId,
                Sender.DisplayName AS SenderDisplayName,
                MemberMessage.DestinationMemberId,
                Destination.DisplayName AS DestinationDisplayName,
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
                chat.Members AS DestinationMember ON DestinationMember.Id = MemberMessage.DestinationMemberId AND DestinationMember.UserId = @UserId

            INNER JOIN
                chat.Members AS SenderMember ON SenderMember.Id = MemberMessage.SenderMemberId

            INNER JOIN
                chat.Users AS Destination ON Destination.Id = DestinationMember.UserId

            INNER JOIN
                chat.Users AS Sender ON Sender.Id = SenderMember.UserId

            WHERE
                Message.ConversationId = @ConversationId
            
            ORDER BY
                Message.SendDate DESC";

        IEnumerable<MessageFromMemberModel> messages = await _dbConnection.QueryAsync<MessageFromMemberModel>(sql, new { ConversationId = conversationId, UserId = userId });

        return messages.ToArray();
    }

    public async Task<UserConversationModel[]> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
	            Conversation.Id AS ConversationId,
                Conversation.Type,
                Conversation.Status,
	            ISNULL(Config.GroupName, OtherUser.DisplayName) AS DisplayName,
	            LastConversationMessage.MessageId,
	            LastConversationMessage.ConversationId,
	            LastConversationMessage.SenderMemberId,
	            LastConversationMessage.SendDate,
	            LastConversationMessage.Content,
	            LastConversationMessage.SenderDisplayName,
                LastConversationMessage.Type,
                LastConversationMessage.Status
	
            FROM 
	            chat.Conversations AS Conversation

            INNER JOIN
	            chat.Members AS Member ON Member.ConversationId = Conversation.Id

            LEFT JOIN
	            chat.GroupConfigs AS Config ON Config.ConversationId = Conversation.Id

            LEFT JOIN
                chat.Members AS OtherMember ON OtherMember.ConversationId = Conversation.Id AND OtherMember.UserId <> @UserId AND Conversation.Type = 'MemberToMember'

            LEFT JOIN
                chat.Users AS OtherUser ON OtherUser.Id = OtherMember.UserId

            OUTER APPLY
            (
                SELECT TOP 1 
                    Message.Id AS MessageId, 
                    Message.ConversationId,
                    Message.SenderMemberId,
                    SenderMember.UserId AS SenderUserId,
                    Message.SendDate,
                    Message.Content,
                    Message.Type,
                    MemberMessage.Status,
                    SenderUser.DisplayName AS SenderDisplayName
                
                FROM 
                    chat.[Messages] AS Message
                
                INNER JOIN
                    chat.MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id AND MemberMessage.DestinationMemberId = Member.Id
                
                INNER JOIN
                    chat.Members AS SenderMember ON SenderMember.Id = Message.SenderMemberId
                
                INNER JOIN
                    chat.Users AS SenderUser ON SenderUser.Id = SenderMember.UserId
                
                WHERE
                    Message.ConversationId = Conversation.Id
                
                ORDER BY 
                    Message.SendDate DESC
            ) AS LastConversationMessage

            WHERE
	            Member.UserId = @UserId";

        IEnumerable<UserConversationModel> userConversations = await _dbConnection.QueryAsync<UserConversationModel, LastMessageFromConversationModel, UserConversationModel>(
            sql, (userConversation, lastMessage) => userConversation with { LastMessage = lastMessage },
            new { UserId = userId },
            splitOn: nameof(LastMessageFromConversationModel.MessageId));

        return userConversations.ToArray();
    }

    public async Task<MessageModel[]> GetMessagesFromUserAsync(Guid userId, MemberMessageStatus? messageStatus, CancellationToken cancellationToken)
    {
        string sql = @$"
            SELECT
                Message.Id AS MessageId,
                Message.ConversationId,
                Message.SenderMemberId,
                Message.SendDate,
                Message.Type,
                Message.Content
            
            FROM 
                chat.Messages AS Message
            
            INNER JOIN
                chat.Members AS Member ON Member.Id = Message.SenderMemberId

            INNER JOIN
                chat.MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id AND MemberMessage.DestinationMemberId = Member.Id
            
            WHERE
                Member.UserId = @UserId {(messageStatus is not null ? "AND MemberMessage.Status = @MessageStatus" : string.Empty)}
            
            ORDER BY
                Message.SendDate DESC";

        IEnumerable<MessageModel> messages = await _dbConnection.QueryAsync<MessageModel>(sql, new { UserId = userId, MessageStatus = messageStatus?.ToString() });

        return messages.ToArray();
    }
}