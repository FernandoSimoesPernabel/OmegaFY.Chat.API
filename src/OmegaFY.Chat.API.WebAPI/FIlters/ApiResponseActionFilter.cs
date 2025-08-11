using Microsoft.AspNetCore.Mvc.Filters;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.WebAPI.FIlters;

public sealed class ApiResponseActionFilter : IActionFilter
{
    private readonly IHostEnvironment _environment;

    public ApiResponseActionFilter(IHostEnvironment environment) => _environment = environment;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        try
        {
            if (context.Result is ObjectResult result && result.Value is GenericResult genericResult)
            {
                if (genericResult.Failed())
                {
                    ApiResponse response = new ApiResponse(genericResult.Errors());
                    context.Result = new ObjectResult(response) { StatusCode = response.StatusCode() };
                    return;
                }

                result.Value = new ApiResponse(result.Value);
            }
        }
        catch (Exception ex)
        {
            ApiResponse response = new ApiResponse(ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR, ex.GetSafeErrorMessageWhenInProd(_environment.IsDevelopment()));
            context.Result = new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}