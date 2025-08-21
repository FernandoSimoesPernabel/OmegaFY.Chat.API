using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities;

public abstract class Entity
{
    public ReferenceId Id { get; }

    protected Entity() => Id = Guid.CreateVersion7();
}