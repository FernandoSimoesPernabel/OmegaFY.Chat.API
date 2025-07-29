using OmegaFY.Chat.API.Infra.MessageBus.Implementations;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Tests.Unit.Infra.MessageBus;

public class InMemoryMessageBusFacts
{
    [Fact]
    public async Task PublishAsync_ShouldHandleConcurrentMessagesToMultipleQueues_WithCorrectlyValuesAndCount()
    {
        // Arrange
        InMemoryMessageBus sut = new InMemoryMessageBus();

        Dictionary<string, int> queuesToTest = new Dictionary<string, int>()
        {
            { "fila_A", 1000 }, 
            { "fila_B", 3234 },
            { "fila_C", 501 },
            { "fila_D", 2150 }
        };

        ParallelOptions parallelOptions = new ParallelOptions()
        {
            CancellationToken = CancellationToken.None,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        // Act
        await Parallel.ForEachAsync(queuesToTest, parallelOptions, async (queue, cancellationToken) =>
        {
            await Parallel.ForEachAsync(Enumerable.Range(0, queue.Value), parallelOptions, async (_, cancellationToken) =>
            {
                await sut.PublishAsync(new MessageEnvelope { DestinationQueue = queue.Key }, cancellationToken);
            });
        });

        // Assert
        foreach (KeyValuePair<string, int> queue in queuesToTest)
            Assert.Equal(queue.Value, sut.GetQueueMessageCount(queue.Key));
    }
}