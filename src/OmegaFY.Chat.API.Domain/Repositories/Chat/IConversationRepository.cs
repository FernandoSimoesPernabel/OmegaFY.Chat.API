using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Repositories.Chat;

public interface IConversationRepository : IRepository<Conversation>
{
    public Task CreateConversationAsync(Conversation conversation, CancellationToken cancellationToken);

    public ValueTask<Conversation> GetConversationByIdAsync(ReferenceId conversationId, CancellationToken cancellationToken);

    public Task<Member> GetMemberAsync(ReferenceId conversationId, ReferenceId userId, CancellationToken cancellationToken);
}