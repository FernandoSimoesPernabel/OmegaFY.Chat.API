using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Models;

namespace OmegaFY.Chat.API.WebAPI.Middlewares;

public sealed class HttpRequestIdempotencyMiddleware : IMiddleware
{
    private readonly IHybridCacheProvider _hybridCacheProvider;
    private const string IDEMPOTENCY_KEY_HEADER = "Idempotency-Key";

    public HttpRequestIdempotencyMiddleware(IHybridCacheProvider hybridCacheProvider) 
        => _hybridCacheProvider = hybridCacheProvider;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method == HttpMethods.Get)
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(IDEMPOTENCY_KEY_HEADER, out var idempotencyKey) || string.IsNullOrWhiteSpace(idempotencyKey))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Idempotency-Key header is required for non-GET requests.");
            return;
        }

        string cacheKey = $"idempotency:{idempotencyKey}";
        
        (bool cacheHit, _) = await _hybridCacheProvider.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                return DateTime.UtcNow;
            },
            new CacheOptions
            {
                Expiration = TimeSpan.FromMinutes(1)
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
