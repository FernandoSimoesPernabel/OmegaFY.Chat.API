using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Infra.MessageBus;

public interface IMessageBus
{
    public Task<MessageEnvelope> ReadMessageAync(string destinationQueue, CancellationToken cancellationToken);

    public Task PublishAsync(MessageEnvelope message, CancellationToken cancellationToken);

    public Task AckAsync(MessageEnvelope message, CancellationToken cancellationToken);

    public Task NackAsync(MessageEnvelope message, CancellationToken cancellationToken);

    public Task RejectAsync(MessageEnvelope message, CancellationToken cancellationToken);
}