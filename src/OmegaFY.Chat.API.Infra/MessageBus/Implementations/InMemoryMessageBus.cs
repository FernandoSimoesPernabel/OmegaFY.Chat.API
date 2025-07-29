using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Collections.Concurrent;

namespace OmegaFY.Chat.API.Infra.MessageBus.Implementations;

internal sealed class InMemoryMessageBus : IMessageBus
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<MessageEnvelope>> _queueStorage = new();

    public Task PublishAsync(MessageEnvelope message, CancellationToken cancellationToken)
    {
        ConcurrentBag<MessageEnvelope> queue = _queueStorage.GetOrAdd(message.DestinationQueue, bag => []);

        queue.Add(message);

        return Task.CompletedTask;
    }

    public Task<MessageEnvelope> ReadMessageAync(string destinationQueue, CancellationToken cancellationToken)
    {
        ConcurrentBag<MessageEnvelope> queue = _queueStorage.GetOrAdd(destinationQueue, bag => []);

        _ = queue.TryTake(out MessageEnvelope message);

        return Task.FromResult(message);
    }

    internal int GetQueueMessageCount(string destinationQueue)
    {
        if (!_queueStorage.TryGetValue(destinationQueue, out ConcurrentBag<MessageEnvelope> queue))
            return 0;

        return queue.Count;
    }
}