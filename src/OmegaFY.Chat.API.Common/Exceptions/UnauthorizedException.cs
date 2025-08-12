using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class UnauthorizedException : DomainErrorCodeException
{
    public UnauthorizedException() : this(string.Empty) { }

    public UnauthorizedException(string message) : base(ApplicationErrorCodesConstants.UNAUTHORIZED, message) { }
}