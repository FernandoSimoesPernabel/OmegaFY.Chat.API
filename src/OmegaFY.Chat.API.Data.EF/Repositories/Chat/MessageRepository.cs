using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Repositories.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Chat;

internal sealed class MessageRepository : RepositoryBase<Message>, IMessageRepository
{
    public MessageRepository(ApplicationContext applicationContext) : base(applicationContext) { }

    public Task CreateMessageAsync(Message message, CancellationToken cancellationToken)
    {
        _dbSet.Add(message);
        return Task.CompletedTask;
    }

    public Task CreateMemberMessageAsync(MemberMessage memberMessage, CancellationToken cancellationToken)
    {
        _context.Set<MemberMessage>().Add(memberMessage);
        return Task.CompletedTask;
    }

    public ValueTask<Message> GetByIdAsync(ReferenceId messageId, CancellationToken cancellationToken) => _dbSet.FindAsync([messageId], cancellationToken);
}