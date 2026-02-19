using OmegaFY.Chat.API.WebAPI.Middlewares;

namespace OmegaFY.Chat.API.WebAPI.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseHttpRequestIdempotencyMiddleware(this IApplicationBuilder app) => app.UseMiddleware<HttpRequestIdempotencyMiddleware>();
}