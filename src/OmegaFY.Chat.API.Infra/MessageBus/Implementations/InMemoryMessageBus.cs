using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Collections.Concurrent;

namespace OmegaFY.Chat.API.Infra.MessageBus.Implementations;

internal sealed class InMemoryMessageBus : IMessageBus
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<MessageEnvelope>> _queueStorage = new();

    public Task AckAsync(MessageEnvelope message, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task NackAsync(MessageEnvelope message, CancellationToken cancellationToken)
    {
        MessageEnvelope retryMessage = message with { RetryCount = message.RetryCount + 1 };

        if (retryMessage.RetryCount >= message.MaxRetryCount)
        {
            await RejectAsync(message, cancellationToken);
            return;
        }
        
        await PublishAsync(message, cancellationToken);
    }

    public async Task RejectAsync(MessageEnvelope message, CancellationToken cancellationToken)
    {
        MessageEnvelope deadLetterMessage = message with { DestinationQueue = message.GetDeadLetterQueue() };

        await PublishAsync(deadLetterMessage, cancellationToken);
    }

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