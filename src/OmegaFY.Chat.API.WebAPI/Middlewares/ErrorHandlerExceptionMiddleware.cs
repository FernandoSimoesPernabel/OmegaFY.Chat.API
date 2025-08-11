using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.WebAPI.Middlewares;

public sealed class ErrorHandlerExceptionMiddleware : IMiddleware
{
    private readonly IHostEnvironment _environment;

    public ErrorHandlerExceptionMiddleware(IHostEnvironment environment) => _environment = environment;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            string erroCode = ex is ErrorCodeException errorCodeException ? errorCodeException.ErrorCode : ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR;

            ApiResponse response = new ApiResponse(erroCode, ex.GetSafeErrorMessageWhenInProd(_environment.IsDevelopment()));

            context.Response.StatusCode = response.StatusCode();
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}