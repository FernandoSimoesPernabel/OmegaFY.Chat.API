using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.WebAPI.Middlewares;

public sealed class HttpRequestIdempotencyMiddleware : IMiddleware
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpRequestIdempotencyMiddleware(IHybridCacheProvider hybridCacheProvider, IHttpContextAccessor httpContextAccessor)
    {
        _hybridCacheProvider = hybridCacheProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method == HttpMethods.Get)
        {
            await next(context);
            return;
        }

        string idempotencyKey = context.GetRequestHeaderByName(HeaderConstants.IDEMPOTENCY_KEY);

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Idempotency-Key header is required for non-GET requests.");
            return;
        }

        string cacheKey = CacheKeyGenerator.IdempotencyKey(_httpContextAccessor.HttpContext.GenerateFingerprint(), idempotencyKey);

        (bool cacheHit, _) = await _hybridCacheProvider.GetOrCreateAsync(
            cacheKey,
            async cancellationToken => DateTime.UtcNow,
            new CacheOptions()
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
