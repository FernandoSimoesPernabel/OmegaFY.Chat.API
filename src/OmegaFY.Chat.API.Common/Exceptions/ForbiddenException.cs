using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class ForbiddenException : ErrorCodeException
{
    public ForbiddenException() : this(string.Empty) { }

    public ForbiddenException(string message) : base(ApplicationErrorCodesConstants.FORBIDDEN, message) { }
}