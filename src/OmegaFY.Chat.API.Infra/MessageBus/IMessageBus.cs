using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Infra.MessageBus;

public interface IMessageBus
{
    public ValueTask<MessageEnvelope> ReadMessageAsync(CancellationToken cancellationToken);

    public ValueTask PublishAsync(MessageEnvelope message, CancellationToken cancellationToken);
}