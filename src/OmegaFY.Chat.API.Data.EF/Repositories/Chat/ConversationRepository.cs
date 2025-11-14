using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Repositories.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Chat;

internal sealed class ConversationRepository : RepositoryBase<Conversation>, IConversationRepository
{
    public ConversationRepository(ApplicationContext applicationContext) : base(applicationContext) { }

    public Task CreateConversationAsync(Conversation conversation, CancellationToken cancellationToken)
    {
        _dbSet.Add(conversation);
        return Task.CompletedTask;
    }

    public ValueTask<Conversation> GetConversationByIdAsync(ReferenceId conversationId, CancellationToken cancellationToken)
        => _dbSet.FindAsync(conversationId, cancellationToken);

    public Task<Member> GetMemberAsync(ReferenceId conversationId, ReferenceId userId, CancellationToken cancellationToken) 
        => _context.Set<Member>().FirstOrDefaultAsync(member => member.ConversationId == conversationId && member.UserId == userId, cancellationToken);
}