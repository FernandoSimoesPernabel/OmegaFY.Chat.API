namespace OmegaFY.Chat.API.Infra.MessageBus.Models;

public sealed class MessageEnvelope
{
    public Guid Id { get; } = Guid.CreateVersion7();

    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public string DestinationQueue { get; init; }

    public string EventType { get; init; }

    public Dictionary<string, string> Headers { get; init; } = [];

    public Dictionary<string, object> Metadata { get; init; } = [];

    public object Payload { get; init; }
}