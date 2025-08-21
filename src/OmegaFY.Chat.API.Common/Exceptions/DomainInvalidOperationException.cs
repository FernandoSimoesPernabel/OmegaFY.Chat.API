using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class DomainInvalidOperationException : DomainException
{
    public DomainInvalidOperationException(string message) : base(ApplicationErrorCodesConstants.INVALID_OPERATION, message) { }
}