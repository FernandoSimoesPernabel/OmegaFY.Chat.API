using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Threading.Channels;

namespace OmegaFY.Chat.API.Infra.MessageBus.Implementations;

internal sealed class ChannelInMemoryMessageBus : IMessageBus
{
    private readonly Channel<MessageEnvelope> _storage = Channel.CreateUnbounded<MessageEnvelope>(new UnboundedChannelOptions()
    {
        AllowSynchronousContinuations = false,
        SingleReader = false,
        SingleWriter = false
    });

    public async ValueTask PublishAsync(MessageEnvelope message, CancellationToken cancellationToken)
        => await _storage.Writer.WriteAsync(message, cancellationToken);

    public ValueTask<MessageEnvelope> ReadMessageAsync(CancellationToken cancellationToken)
    {
        _storage.Reader.TryRead(out MessageEnvelope message);
        return ValueTask.FromResult(message);
    }

    internal int GetMessageCount() => _storage.Reader.CanCount ? _storage.Reader.Count : 0;
}