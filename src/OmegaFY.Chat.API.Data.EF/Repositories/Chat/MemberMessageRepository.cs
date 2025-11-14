using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Repositories.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Chat;

internal sealed class MemberMessageRepository : RepositoryBase<MemberMessage>, IMemberMessageRepository
{
    public MemberMessageRepository(ApplicationContext applicationContext) : base(applicationContext) { }

    public Task CreateMemberMessageAsync(MemberMessage memberMessage, CancellationToken cancellationToken)
    {
        _dbSet.Add(memberMessage);
        return Task.CompletedTask;
    }

    public Task<MemberMessage> GetMemberMessageAsync(ReferenceId messageId, ReferenceId memberId, CancellationToken cancellationToken)
        => _dbSet.FirstOrDefaultAsync(message => message.MessageId == messageId && message.DestinationMemberId == memberId, cancellationToken);
}