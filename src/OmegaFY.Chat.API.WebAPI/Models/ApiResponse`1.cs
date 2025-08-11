using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;

namespace OmegaFY.Chat.API.WebAPI.Models;

public class ApiResponse<T>
{
    private readonly List<ValidationError> _errors;

    public bool Succeeded => _errors?.Count == 0;

    public IReadOnlyCollection<ValidationError> Errors => _errors.AsReadOnly();

    public T Data { get; set; }

    public ApiResponse() => _errors = new List<ValidationError>();

    public ApiResponse(T data) : this() => Data = data;

    public ApiResponse(IReadOnlyCollection<ValidationError> errors) : this() => _errors = new List<ValidationError>(errors);

    public ApiResponse(string code, string mesage) : this() => _errors.Add(new ValidationError(code, mesage));

    public int StatusCode()
    {
        if (Succeeded)
            return StatusCodes.Status200OK;

        if (_errors.Any(erro => erro.Code.In(ApplicationErrorCodesConstants.GENERIC_DOMAIN_ERROR,
                                             ApplicationErrorCodesConstants.INVALID_OPERATION,
                                             ApplicationErrorCodesConstants.DOMAIN_ARGUMENT_INVALID,
                                             ApplicationErrorCodesConstants.UNABLE_TO_CREATE_USER_ON_IDENTITY)))
            return StatusCodes.Status400BadRequest;

        if (_errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.NOT_FOUND))
            return StatusCodes.Status404NotFound;

        if (_errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.ENTITY_CONFLICTED))
            return StatusCodes.Status409Conflict;

        if (_errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.UNAUTHORIZED))
            return StatusCodes.Status401Unauthorized;

        return StatusCodes.Status500InternalServerError;
    }
}