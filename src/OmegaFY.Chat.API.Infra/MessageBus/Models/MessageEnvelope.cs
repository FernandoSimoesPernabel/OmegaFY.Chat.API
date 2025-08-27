using OmegaFY.Chat.API.Infra.Constants;

namespace OmegaFY.Chat.API.Infra.MessageBus.Models;

public sealed record class MessageEnvelope
{
    public Guid Id { get; } = Guid.CreateVersion7();

    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public int RetryCount { get; init; }

    public int MaxRetryCount { get; init; } = QueueConstants.DEFAULT_MAX_RETRY_COUNT;

    public string DestinationQueue { get; init; }

    public string EventType { get; init; }

    public Dictionary<string, string> Headers { get; init; } = [];

    public object Payload { get; init; }
}