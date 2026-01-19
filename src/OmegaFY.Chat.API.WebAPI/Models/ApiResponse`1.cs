using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;

namespace OmegaFY.Chat.API.WebAPI.Models;

public class ApiResponse<T>
{
    public bool Succeeded => Errors.Length == 0;

    public ValidationError[] Errors { get; init; } = [];

    public T Data { get; init; }

    public ApiResponse() => Errors = [];

    public ApiResponse(T data) : this() => Data = data;

    public ApiResponse(ValidationError[] errors) : this() => Errors = errors ?? [];

    public ApiResponse(string code, string message) : this() => Errors = [new ValidationError(code, message)];

    public int StatusCode()
    {
        if (Succeeded)
            return StatusCodes.Status200OK;

        if (Errors.Any(erro => erro.Code.In(ApplicationErrorCodesConstants.GENERIC_DOMAIN_ERROR,
                                            ApplicationErrorCodesConstants.INVALID_OPERATION,
                                            ApplicationErrorCodesConstants.DOMAIN_ARGUMENT_INVALID,
                                            ApplicationErrorCodesConstants.UNABLE_TO_CREATE_USER_ON_IDENTITY)))
            return StatusCodes.Status400BadRequest;

        if (Errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.NOT_FOUND))
            return StatusCodes.Status404NotFound;

        if (Errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.ENTITY_CONFLICTED))
            return StatusCodes.Status409Conflict;

        if (Errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.UNAUTHORIZED))
            return StatusCodes.Status401Unauthorized;

        if (Errors.Any(erro => erro.Code == ApplicationErrorCodesConstants.UNAUTHENTICATED))
            return StatusCodes.Status403Forbidden;

        return StatusCodes.Status500InternalServerError;
    }
}