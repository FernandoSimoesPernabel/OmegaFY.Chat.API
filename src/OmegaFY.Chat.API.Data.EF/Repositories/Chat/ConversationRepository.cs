using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Repositories.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Chat;

internal sealed class ConversationRepository : RepositoryBase<Conversation>, IConversationRepository
{
    public ConversationRepository(ApplicationContext applicationContext) : base(applicationContext) { }

    public Task CreateConversationAsync(Conversation conversation, CancellationToken cancellationToken)
    {
        _dbSet.Add(conversation);
        return Task.CompletedTask;
    }
}