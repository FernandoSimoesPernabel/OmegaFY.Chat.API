namespace OmegaFY.Chat.API.Infra.MessageBus.Models;

public sealed record class MessageEnvelope
{
    public Guid Id { get; } = Guid.CreateVersion7();

    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public Dictionary<string, string> Headers { get; init; } = [];

    public object Payload { get; init; }
}