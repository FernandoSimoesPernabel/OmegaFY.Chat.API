using FluentValidation.Results;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class ValidationResultExtensions
{
    public static HandlerResult<TResult> ToHandlerResult<TResult>(this ValidationResult validationResult) 
    {
        HandlerResult<TResult> handlerResult = Activator.CreateInstance<HandlerResult<TResult>>();

        foreach (ValidationFailure failure in validationResult.Errors)
            handlerResult.AddError(ApplicationErrorCodesConstants.DOMAIN_ARGUMENT_INVALID, failure.ErrorMessage);
        
        return handlerResult;
    } 
}