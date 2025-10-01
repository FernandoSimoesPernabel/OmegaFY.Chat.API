using Microsoft.AspNetCore.Mvc.Filters;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.WebAPI.Filters;

public sealed class ErrorHandlerExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _environment;

    public ErrorHandlerExceptionFilter(IHostEnvironment environment) => _environment = environment;

    public void OnException(ExceptionContext context)
    {
        ApiResponse response = new ApiResponse(context.Exception.GetErrorCode(), context.Exception.GetSafeErrorMessageWhenInProd(_environment.IsDevelopment()));
        context.Result = new ObjectResult(response) { StatusCode = response.StatusCode() };
    }
}