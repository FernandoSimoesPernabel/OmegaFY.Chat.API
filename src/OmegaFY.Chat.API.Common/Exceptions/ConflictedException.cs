using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Exceptions;

public sealed class ConflictedException : ErrorCodeException
{
    public ConflictedException() : this(string.Empty) { }

    public ConflictedException(string message) : base(ApplicationErrorCodesConstants.ENTITY_CONFLICTED, message) { }
}