using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class DomainArgumentException : DomainException
{
    public DomainArgumentException(string message) : this(ApplicationErrorCodesConstants.DOMAIN_ARGUMENT_INVALID, message) { }

    public DomainArgumentException(string erroCode, string message) : base(message) => ErrorCode = erroCode;
}