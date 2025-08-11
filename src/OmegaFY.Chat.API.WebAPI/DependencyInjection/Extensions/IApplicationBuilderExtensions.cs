using OmegaFY.Chat.API.WebAPI.Middlewares;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Extensions;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseErrorHandlerExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlerExceptionMiddleware>();
    }
}