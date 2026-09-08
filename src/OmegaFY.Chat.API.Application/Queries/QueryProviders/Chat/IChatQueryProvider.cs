using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;

public interface IChatQueryProvider
{
    public Task<ConversationAndMembersModel> GetConversationByIdAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken);
    
    public Task<MemberModel> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken);
    
    public Task<MessageFromMemberModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);

    public Task<(MessageFromMemberModel[] messageFromMembers, CursorPaginationResultInfo<DateTime> paginationInfo)> GetMessagesFromMemberAsync(Guid conversationId, Guid userId, CursorPagination<DateTime> pagination, CancellationToken cancellationToken);
   
    public Task<(MessageModel[] messageFromMembers, PaginationResultInfo paginationInfo)> GetMessagesFromUserAsync(Guid userId, MemberMessageStatus? messageStatus, Pagination pagination, CancellationToken cancellationToken);
    
    public Task<UserConversationModel[]> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken);
}