using OmegaFY.Chat.API.Common.Exceptions;

namespace OmegaFY.Chat.API.Domain.ValueObjects.Shared;

public readonly record struct ReferenceId
{
    public Guid Value { get; }

    public ReferenceId(Guid id)
    {
        if (id == Guid.Empty)
            throw new DomainArgumentException("O Id precisa ser informado para um ReferenceId.");

        Value = id;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(ReferenceId referenceId) => referenceId.Value;

    public static implicit operator ReferenceId(Guid guid) => new ReferenceId(guid);
}
