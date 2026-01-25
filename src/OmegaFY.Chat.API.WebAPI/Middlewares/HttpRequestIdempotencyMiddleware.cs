using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.Constants;

namespace OmegaFY.Chat.API.WebAPI.Middlewares;

public sealed class HttpRequestIdempotencyMiddleware : IMiddleware
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public HttpRequestIdempotencyMiddleware(IHybridCacheProvider hybridCacheProvider) 
        => _hybridCacheProvider = hybridCacheProvider;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method == HttpMethods.Get)
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(HeaderConstants.IDEMPOTENCY_KEY, out Microsoft.Extensions.Primitives.StringValues idempotencyKey) || string.IsNullOrWhiteSpace(idempotencyKey))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Idempotency-Key header is required for non-GET requests.");
            return;
        }

        string cacheKey = CacheKeyGenerator.IdempotencyKey(idempotencyKey!);
        
        (bool cacheHit, _) = await _hybridCacheProvider.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                return DateTime.UtcNow;
            },
            new CacheOptions
            {
                Expiration = TimeSpanConstants.ONE_MINUTE
            },
            context.RequestAborted);

        if (cacheHit)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync("Request with this Idempotency-Key has already been processed.");
            return;
        }

        await next(context);
    }
}
