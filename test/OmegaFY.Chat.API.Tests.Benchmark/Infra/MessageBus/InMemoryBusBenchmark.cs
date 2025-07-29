using BenchmarkDotNet.Attributes;
using OmegaFY.Chat.API.Infra.MessageBus.Implementations;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Tests.Benchmark.Infra.MessageBus;

[MemoryDiagnoser]
public class InMemoryBusBenchmark
{
    private InMemoryMessageBus _messageBus = null;

    private MessageEnvelope[] _messages = null;

    [Params(100_000, 1_000_000)]
    public int TotalMessages { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _messageBus = new InMemoryMessageBus();

        _messages = Enumerable.Range(0, TotalMessages).Select(i => new MessageEnvelope { DestinationQueue = "benchmark-queue", Payload = i }).ToArray();
    }

    [Benchmark]
    public async Task PublishParallelAsync()
    {
        ParallelOptions parallelOptions = new ParallelOptions
        {
            CancellationToken = CancellationToken.None,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        await Parallel.ForEachAsync(_messages, parallelOptions, async (message, cancellationToken) =>
        {
            await _messageBus.PublishAsync(message, cancellationToken);
        });
    }
}
