using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class UserLockedOutException : ErrorCodeException
{
    public UserLockedOutException() : this(string.Empty) { }

    public UserLockedOutException(string message) : base(ApplicationErrorCodesConstants.USER_LOCKED_OUT, message) { }
}