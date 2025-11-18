using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;

public interface IChatQueryProvider
{
    public Task<ConversationAndMembersModel> GetConversationByIdAsync(Guid conversationId, CancellationToken cancellationToken);
    
    public Task<MemberModel> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken);
    
    public Task<MemberAndMessageModel> GetMessageFromMemberAsync(Guid messageId, Guid userId, CancellationToken cancellationToken);
}