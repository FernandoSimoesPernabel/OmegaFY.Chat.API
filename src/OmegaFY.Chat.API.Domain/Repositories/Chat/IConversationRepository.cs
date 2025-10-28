using OmegaFY.Chat.API.Domain.Entities.Chat;

namespace OmegaFY.Chat.API.Domain.Repositories.Chat;

public interface IConversationRepository : IRepository<Conversation>
{
    public Task CreateConversationAsync(Conversation conversation, CancellationToken cancellationToken);
}