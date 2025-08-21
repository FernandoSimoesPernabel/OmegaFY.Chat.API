using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class UnauthenticatedException : ErrorCodeException
{
    public UnauthenticatedException() : this(string.Empty) { }

    public UnauthenticatedException(string message) : base(ApplicationErrorCodesConstants.UNAUTHENTICATED, message) { }
}