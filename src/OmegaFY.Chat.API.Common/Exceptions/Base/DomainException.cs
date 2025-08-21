using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions.Base;

public abstract class DomainException : ErrorCodeException
{
    public DomainException(string message) : this(ApplicationErrorCodesConstants.GENERIC_DOMAIN_ERROR, message) { }

    public DomainException(string erroCode, string message) : base(erroCode, message) { }
}