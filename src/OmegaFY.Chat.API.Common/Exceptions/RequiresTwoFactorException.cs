using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class RequiresTwoFactorException : ErrorCodeException
{
    public RequiresTwoFactorException() : this(string.Empty) { }

    public RequiresTwoFactorException(string message) : base(ApplicationErrorCodesConstants.UNAUTHORIZED, message) { }
}