using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class UnableToCreateUserOnIdentityException : DomainErrorCodeException
{
    public UnableToCreateUserOnIdentityException() : this(string.Empty) { }

    public UnableToCreateUserOnIdentityException(string message) : base(ApplicationErrorCodesConstants.UNABLE_TO_CREATE_USER_ON_IDENTITY, message) { }
}