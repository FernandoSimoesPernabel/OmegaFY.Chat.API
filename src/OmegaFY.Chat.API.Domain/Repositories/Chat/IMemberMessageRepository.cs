using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Repositories.Chat;

public interface IMemberMessageRepository : IRepository<MemberMessage>
{
    public Task CreateMemberMessageAsync(MemberMessage memberMessage, CancellationToken cancellationToken);

    public Task<MemberMessage> GetMemberMessageAsync(ReferenceId messageId, ReferenceId memberId, CancellationToken cancellationToken);
}