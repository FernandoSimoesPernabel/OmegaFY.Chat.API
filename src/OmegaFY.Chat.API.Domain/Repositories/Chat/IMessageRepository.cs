using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Repositories.Chat;

public interface IMessageRepository : IRepository<Message>
{
    public Task CreateMessageAsync(Message message, CancellationToken cancellationToken);

    public ValueTask<Message> GetMessageByIdAsync(ReferenceId messageId, CancellationToken cancellationToken);


}