using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;

public interface IChatQueryProvider
{
    public Task<ConversationAndMembersModel> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken);
    
    public Task<MemberModel> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken);
    
    public Task<MessageFromMemberModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);

    public Task<MessageFromMemberModel[]> GetMessagesFromMemberAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken);
   
    public Task<MessageModel[]> GetMessagesFromUserAsync(Guid userId, MemberMessageStatus? messageStatus, CancellationToken cancellationToken);
    
    public Task<UserConversationModel[]> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken);
}