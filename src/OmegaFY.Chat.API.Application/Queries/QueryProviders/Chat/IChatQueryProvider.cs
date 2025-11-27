using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;

public interface IChatQueryProvider
{
    public Task<ConversationAndMembersModel> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken);
    
    public Task<MemberModel> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken);
    
    public Task<MessageFromMemberModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);

    public Task<(MessageFromMemberModel[], PaginationResultInfo paginationInfo)> GetMessagesFromMemberAsync(Guid conversationId, Guid userId, Pagination pagination, CancellationToken cancellationToken);
   
    public Task<(MessageModel[], PaginationResultInfo paginationInfo)> GetMessagesFromUserAsync(Guid userId, MemberMessageStatus? messageStatus, Pagination pagination, CancellationToken cancellationToken);
    
    public Task<UserConversationModel[]> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken);
}