using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class NotFoundException : DomainErrorCodeException
{
    public NotFoundException() : this(string.Empty) { }

    public NotFoundException(string message) : base(ApplicationErrorCodesConstants.NOT_FOUND, message) { }
}