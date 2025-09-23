using BenchmarkDotNet.Attributes;
using OmegaFY.Chat.API.Infra.MessageBus.Implementations;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
namespace OmegaFY.Chat.API.Tests.Benchmark.Infra.MessageBus;

[MemoryDiagnoser]
public class InMemoryMessageBusBenchmark
{
    private ChannelInMemoryMessageBus _channelInMemoryMessageBus = null;

    private ConcurrentBagInMemoryMessageBus _concurrentBagInMemoryMessageBus = null;

    private MessageEnvelope[] _messages = null;

    [Params(1, 10, 100, 1000)]
    public int TotalMessages { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _channelInMemoryMessageBus = new ChannelInMemoryMessageBus();

        _concurrentBagInMemoryMessageBus = new ConcurrentBagInMemoryMessageBus();

        _messages = Enumerable.Range(0, TotalMessages).Select(i => new MessageEnvelope { Payload = i }).ToArray();
    }

    [Benchmark]
    public async Task ChannelInMemoryMessageBusPublishAsync()
    {
        ParallelOptions parallelOptions = new ParallelOptions
        {
            CancellationToken = CancellationToken.None,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        await Parallel.ForEachAsync(_messages, parallelOptions, async (message, cancellationToken) =>
        {
            await _channelInMemoryMessageBus.PublishAsync(message, cancellationToken);
        });
    }

    [Benchmark]
    public async Task ConcurrentBagInMemoryMessageBusPublishAsync()
    {
        ParallelOptions parallelOptions = new ParallelOptions
        {
            CancellationToken = CancellationToken.None,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        await Parallel.ForEachAsync(_messages, parallelOptions, async (message, cancellationToken) =>
        {
            await _concurrentBagInMemoryMessageBus.PublishAsync(message, cancellationToken);
        });
    }
}