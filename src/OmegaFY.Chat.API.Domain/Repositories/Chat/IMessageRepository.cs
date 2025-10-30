using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Repositories.Chat;

public interface IMessageRepository : IRepository<Message>
{
    public Task CreateMessageAsync(Message message, CancellationToken cancellationToken);

    public Task CreateMemberMessageAsync(MemberMessage memberMessage, CancellationToken cancellationToken);

    public ValueTask<Message> GetByIdAsync(ReferenceId messageId, CancellationToken cancellationToken);
}