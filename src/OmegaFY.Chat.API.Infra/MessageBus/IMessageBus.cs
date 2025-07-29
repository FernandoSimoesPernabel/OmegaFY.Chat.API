using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Infra.MessageBus;

public interface IMessageBus
{
    public Task<MessageEnvelope> ReadMessageAync(string destinationQueue, CancellationToken cancellationToken);

    public Task PublishAsync(MessageEnvelope message, CancellationToken cancellationToken);
}