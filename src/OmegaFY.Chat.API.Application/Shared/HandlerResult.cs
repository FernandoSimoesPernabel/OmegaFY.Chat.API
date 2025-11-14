using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Common.Exceptions.Base;

namespace OmegaFY.Chat.API.Application.Shared;

public abstract record class HandlerResult
{
    private readonly List<ValidationError> _errors;

    public HandlerResult() => _errors = new List<ValidationError>(2);

    public HandlerResult(string code, string message) : this() => AddError(code, message);

    public void AddError(string code, string message) => _errors.Add(new ValidationError(code, message));

    public bool Succeeded() => _errors?.Count == 0;

    public bool Failed() => !Succeeded();

    public IReadOnlyCollection<ValidationError> Errors() => _errors.AsReadOnly();

    public string GetErrorsAsStringSeparatedByNewLine() => string.Join(Environment.NewLine, _errors.Select(error => error.Message));

    public static HandlerResult<TResult> Create<TResult>(TResult result) => new HandlerResult<TResult>(result);

    public static HandlerResult<TResult> CreateUnauthorized<TResult>() => CreateError<TResult>(new UnauthorizedException());

    public static HandlerResult<TResult> CreateUnauthenticated<TResult>() => CreateError<TResult>(new UnauthenticatedException());

    public static HandlerResult<TResult> CreateNotFound<TResult>() => CreateError<TResult>(new NotFoundException());

    public static HandlerResult<TResult> CreateError<TResult>(ErrorCodeException ex) => new HandlerResult<TResult>(ex.ErrorCode, ex.Message);
}