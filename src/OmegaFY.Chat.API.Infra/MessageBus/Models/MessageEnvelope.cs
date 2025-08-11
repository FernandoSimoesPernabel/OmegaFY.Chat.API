using OmegaFY.Chat.API.Infra.MessageBus.Enums;

namespace OmegaFY.Chat.API.Infra.MessageBus.Models;

public sealed class MessageEnvelope
{
    public Guid Id { get; } = Guid.CreateVersion7();

    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public string DestinationQueue { get; init; }

    public MessageType Type { get; init; }

    public string Sender { get; init; }

    public KeyValuePair<string, string>[] Headers { get; init; } = [];

    public KeyValuePair<string, object>[] Metadata { get; init; } = [];

    public object Payload { get; init; }
}