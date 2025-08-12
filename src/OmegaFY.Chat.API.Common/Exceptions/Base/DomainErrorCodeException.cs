namespace OmegaFY.Chat.API.Common.Exceptions.Base;

public abstract class DomainErrorCodeException : Exception
{
    public string ErrorCode { get; init; }

    public DomainErrorCodeException(string erroCode, string message) : base(message) => ErrorCode = erroCode;
}