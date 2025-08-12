using BenchmarkDotNet.Running;
using OmegaFY.Chat.API.Tests.Benchmark.Infra.MessageBus;

namespace OmegaFY.Chat.API.Tests.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<InMemoryBusBenchmark>();
    }
}