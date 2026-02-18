using Dapper;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Domain.Enums;
using System.Data;

namespace OmegaFY.Chat.API.Data.Dapper.QueryProviders.Chat;

internal sealed class ChatQueryProvider : IChatQueryProvider
{
	private readonly IDbConnection _dbConnection;

	public ChatQueryProvider(IDbConnection dbConnection) => _dbConnection = dbConnection;

	public async Task<ConversationAndMembersModel> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		const string sql = @"
			SELECT
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
				Conversations AS C

			LEFT JOIN
				GroupConfigs AS GC ON C.Id = GC.ConversationId 

			WHERE 
				C.Id = @ConversationId

			LIMIT 1;

			SELECT
				M.Id AS MemberId,
				M.ConversationId,
				M.UserId, 
				M.JoinedDate

			FROM
				Members AS M

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
		cancellationToken.ThrowIfCancellationRequested();

		const string sql = @"
			SELECT
				M.Id AS MemberId,
				M.ConversationId,
				M.UserId, 
				M.JoinedDate

			FROM
				Members AS M

			WHERE
				M.Id = @MemberId

			LIMIT 1";

		return await _dbConnection.QueryFirstOrDefaultAsync<MemberModel>(sql, new { MemberId = memberId });
	}

	public async Task<MessageFromMemberModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

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
				Messages AS Message

			INNER JOIN
				MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id

			INNER JOIN
				Members AS DestinationMember ON DestinationMember.Id = MemberMessage.DestinationMemberId AND DestinationMember.UserId = @UserId

			INNER JOIN
				Members AS SenderMember ON SenderMember.Id = MemberMessage.SenderMemberId

			INNER JOIN
				Users AS Destination ON Destination.Id = DestinationMember.UserId

			INNER JOIN
				Users AS Sender ON Sender.Id = SenderMember.UserId

			WHERE
				Message.Id = @MessageId

			LIMIT 1";

		return await _dbConnection.QueryFirstOrDefaultAsync<MessageFromMemberModel>(sql, new { MessageId = messageId, UserId = userId });
	}

	public async Task<(MessageFromMemberModel[], PaginationResultInfo paginationInfo)> GetMessagesFromMemberAsync(Guid conversationId, Guid userId, Pagination pagination, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		const string baseSqlQuery = @"
			FROM
				Messages AS Message

			INNER JOIN
				MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id

			INNER JOIN
				Members AS DestinationMember ON DestinationMember.Id = MemberMessage.DestinationMemberId AND DestinationMember.UserId = @UserId

			INNER JOIN
				Members AS SenderMember ON SenderMember.Id = MemberMessage.SenderMemberId

			INNER JOIN
				Users AS Destination ON Destination.Id = DestinationMember.UserId

			INNER JOIN
				Users AS Sender ON Sender.Id = SenderMember.UserId

			WHERE
				Message.ConversationId = @ConversationId";

		long totalOfItems = await _dbConnection.ExecuteScalarAsync<long>($"SELECT COUNT(*) {baseSqlQuery}", new { ConversationId = conversationId, UserId = userId });

		const string sql = @$"
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

			{baseSqlQuery}

			ORDER BY
				Message.SendDate DESC

			LIMIT @Take OFFSET @Skip";

		PaginationResultInfo paginationInfo = new PaginationResultInfo(pagination.PageNumber, pagination.PageSize, totalOfItems);

		IEnumerable<MessageFromMemberModel> messages = await _dbConnection.QueryAsync<MessageFromMemberModel>(
			sql, 
			new { ConversationId = conversationId, UserId = userId, Skip = paginationInfo.ItemsToSkip(), Take = paginationInfo.PageSize });

		return (messages.ToArray(), paginationInfo);
	}

	public async Task<UserConversationModel[]> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		const string sql = @"
			SELECT
				Conversation.Id AS ConversationId,
				Conversation.Type,
				Conversation.Status,
				COALESCE(Config.GroupName, OtherUser.DisplayName) AS DisplayName,
				LastMessage.MessageId,
				LastMessage.ConversationId AS LastMessageConversationId,
				LastMessage.SenderMemberId,
				LastMessage.SendDate,
				LastMessage.Content,
				LastMessage.SenderDisplayName,
				LastMessage.Type AS LastMessageType,
				LastMessage.Status AS LastMessageStatus

			FROM 
				Conversations AS Conversation

			INNER JOIN
				Members AS Member ON Member.ConversationId = Conversation.Id

			LEFT JOIN
				GroupConfigs AS Config ON Config.ConversationId = Conversation.Id

			LEFT JOIN
				Members AS OtherMember ON OtherMember.ConversationId = Conversation.Id AND OtherMember.UserId <> @UserId AND Conversation.Type = 'MemberToMember'

			LEFT JOIN
				Users AS OtherUser ON OtherUser.Id = OtherMember.UserId

			LEFT JOIN
			(
				SELECT 
					Message.Id AS MessageId, 
					Message.ConversationId,
					Message.SenderMemberId,
					SenderMember.UserId AS SenderUserId,
					Message.SendDate,
					Message.Content,
					Message.Type,
					MemberMessage.Status,
					MemberMessage.DestinationMemberId,
					SenderUser.DisplayName AS SenderDisplayName,
					ROW_NUMBER() OVER (PARTITION BY Message.ConversationId, MemberMessage.DestinationMemberId ORDER BY Message.SendDate DESC) AS RowNum

				FROM 
					Messages AS Message

				INNER JOIN
					MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id

				INNER JOIN
					Members AS SenderMember ON SenderMember.Id = Message.SenderMemberId

				INNER JOIN
					Users AS SenderUser ON SenderUser.Id = SenderMember.UserId
			) AS LastMessage ON LastMessage.ConversationId = Conversation.Id AND LastMessage.DestinationMemberId = Member.Id AND LastMessage.RowNum = 1

			WHERE
				Member.UserId = @UserId

			ORDER BY
				LastMessage.SendDate DESC,
				Conversation.CreatedDate DESC";

		IEnumerable<UserConversationModel> userConversations = await _dbConnection.QueryAsync<UserConversationModel, LastMessageFromConversationModel, UserConversationModel>(
			sql, (userConversation, lastMessage) => userConversation with { LastMessage = lastMessage },
			new { UserId = userId },
			splitOn: nameof(LastMessageFromConversationModel.MessageId));

		return userConversations.ToArray();
	}

	public async Task<(MessageModel[], PaginationResultInfo paginationInfo)> GetMessagesFromUserAsync(Guid userId, MemberMessageStatus? messageStatus, Pagination pagination, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		string baseSqlQuery = @$"
			FROM 
				Messages AS Message

			INNER JOIN
				Members AS Member ON Member.Id = Message.SenderMemberId

			INNER JOIN
				MemberMessages AS MemberMessage ON MemberMessage.MessageId = Message.Id AND MemberMessage.DestinationMemberId = Member.Id

			WHERE
				Member.UserId = @UserId {(messageStatus is not null ? "AND MemberMessage.Status = @MessageStatus" : string.Empty)}";

		long totalOfItems = await _dbConnection.ExecuteScalarAsync<long>($"SELECT COUNT(*) {baseSqlQuery}", new { UserId = userId, MessageStatus = messageStatus?.ToString() });

		string sql = @$"
			SELECT
				Message.Id AS MessageId,
				Message.ConversationId,
				Message.SenderMemberId,
				Message.SendDate,
				Message.Type,
				Message.Content

			{baseSqlQuery}

			ORDER BY
				Message.SendDate DESC

			LIMIT @Take OFFSET @Skip";

		PaginationResultInfo paginationInfo = new PaginationResultInfo(pagination.PageNumber, pagination.PageSize, totalOfItems);

		IEnumerable<MessageModel> messages = await _dbConnection.QueryAsync<MessageModel>(
			sql, 
			new { UserId = userId, MessageStatus = messageStatus?.ToString(), Skip = paginationInfo.ItemsToSkip(), Take = paginationInfo.PageSize });

		return (messages.ToArray(), paginationInfo);
	}
}