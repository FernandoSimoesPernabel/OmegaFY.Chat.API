using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.ValueConverts;

internal sealed class ReferenceIdValueConverter : ValueConverter<ReferenceId, Guid>
{
    public ReferenceIdValueConverter() : base(referenceId => referenceId.Value, referenceId => new ReferenceId(referenceId))
    {
    }
}
