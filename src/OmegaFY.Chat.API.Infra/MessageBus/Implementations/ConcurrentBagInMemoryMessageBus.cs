using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Collections.Concurrent;

namespace OmegaFY.Chat.API.Infra.MessageBus.Implementations;

internal sealed class ConcurrentBagInMemoryMessageBus : IMessageBus
{
    private readonly ConcurrentBag<MessageEnvelope> _storage = new();

    public ValueTask PublishAsync(MessageEnvelope message, CancellationToken cancellationToken)
    {
        _storage.Add(message);
        return ValueTask.CompletedTask;
    }

    public ValueTask<MessageEnvelope> ReadMessageAsync(CancellationToken cancellationToken)
    {
        _storage.TryTake(out MessageEnvelope message);
        return ValueTask.FromResult(message);
    }

    internal int GetMessageCount() => _storage.Count;
}