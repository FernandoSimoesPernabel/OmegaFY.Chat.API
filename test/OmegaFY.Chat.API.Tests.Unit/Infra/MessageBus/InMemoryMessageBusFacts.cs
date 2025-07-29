using Microsoft.AspNetCore.Http;
using OmegaFY.Chat.API.Infra.MessageBus.Enums;
using OmegaFY.Chat.API.Infra.MessageBus.Implementations;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Collections.Concurrent;

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

    [Fact]
    public async Task ReadMessageAync_ShouldReturnMessageAndDecreaseQueueCount_WhenQueueIsNotEmpty()
    {
        // Arrange
        InMemoryMessageBus sut = new InMemoryMessageBus();
        const string queueName = "read-test-queue";
        MessageEnvelope message = new MessageEnvelope { DestinationQueue = queueName, Payload = "Hello World" };
        
        await sut.PublishAsync(message, CancellationToken.None);

        // Act
        int publishQueueMessageCount = sut.GetQueueMessageCount(queueName);
        MessageEnvelope receivedMessage = await sut.ReadMessageAync(queueName, CancellationToken.None);

        // Assert
        Assert.NotNull(receivedMessage);
        Assert.Equal(message.Id, receivedMessage.Id);
        Assert.Equal("Hello World", receivedMessage.Payload);
        Assert.Equal(1, publishQueueMessageCount);
        Assert.Equal(0, sut.GetQueueMessageCount(queueName));
    }

    [Fact]
    public async Task ReadMessageAync_ShouldReturnNull_WhenQueueIsEmpty()
    {
        // Arrange
        InMemoryMessageBus sut = new InMemoryMessageBus();
        const string queueName = "empty-queue";

        // Act
        MessageEnvelope receivedMessage = await sut.ReadMessageAync(queueName, CancellationToken.None);

        // Assert
        Assert.Null(receivedMessage);
    }

    [Fact]
    public async Task ProducerConsumer_ShouldProcessAllMessages_UnderParallelLoad()
    {
        // Arrange
        InMemoryMessageBus sut = new InMemoryMessageBus();
        ConcurrentBag<MessageEnvelope> consumedMessages = new ConcurrentBag<MessageEnvelope>();
        const string queueName = "parallel-producer-consumer-queue";
        const int messagesToProcess = 2000;

        ParallelOptions parallelOptions = new ParallelOptions
        {
            CancellationToken = CancellationToken.None,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        // Act
        // Produtores: Múltiplas "threads" publicam mensagens em paralelo.
        Task producerTask = Parallel.ForEachAsync(Enumerable.Range(0, messagesToProcess), parallelOptions, async (index, _) =>
        {
            await sut.PublishAsync(new MessageEnvelope { DestinationQueue = queueName, Payload = index }, CancellationToken.None);
        });

        Task consumerTask = Task.Run(async () =>
        {
            while (consumedMessages.Count < messagesToProcess)
            {
                MessageEnvelope message = await sut.ReadMessageAync(queueName, CancellationToken.None);
                
                if (message is not null)
                {
                    consumedMessages.Add(message);
                    continue;
                }

                // Se a fila está vazia, espera um pouco. Apenas um laço de polling para gerenciar.
                await Task.Delay(5);
            }
        });

        // Espera no máximo um tempo razoável (ex: 5 segundos) para tudo terminar
        await Task.WhenAll(producerTask, consumerTask).WaitAsync(TimeSpan.FromSeconds(10));

        // Assert
        Assert.Equal(messagesToProcess, consumedMessages.Count);
        Assert.Equal(0, sut.GetQueueMessageCount(queueName));
    }

    [Fact]
    public async Task PublishAndRead_ShouldPreserveMessageIntegrity()
    {
        // Arrange
        InMemoryMessageBus sut = new InMemoryMessageBus();
        MessageEnvelope originalMessage = new MessageEnvelope
        {
            DestinationQueue = "integrity-test",
            Sender = "Test Sender",
            Type = MessageType.RegisterNewUser,
            Payload = new { OrderId = 123, Amount = 99.95, CreatedDate = DateTime.UtcNow }
        };

        // Act
        await sut.PublishAsync(originalMessage, CancellationToken.None);
        MessageEnvelope receivedMessage = await sut.ReadMessageAync(originalMessage.DestinationQueue, CancellationToken.None);

        // Assert
        Assert.NotNull(receivedMessage);
        Assert.Equal(originalMessage.Id, receivedMessage.Id);
        Assert.Equal(originalMessage.Sender, receivedMessage.Sender);
        Assert.Equal(originalMessage.Type, receivedMessage.Type);
        Assert.Equal(originalMessage.Payload, receivedMessage.Payload);
    }
}