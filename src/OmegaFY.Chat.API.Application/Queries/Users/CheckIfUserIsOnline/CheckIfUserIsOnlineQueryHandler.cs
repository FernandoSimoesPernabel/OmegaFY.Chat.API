using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.CheckIfUserIsOnline;

public sealed class CheckIfUserIsOnlineQueryHandler : QueryHandlerBase<CheckIfUserIsOnlineQueryHandler, CheckIfUserIsOnlineQuery, CheckIfUserIsOnlineQueryResult>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public CheckIfUserIsOnlineQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<CheckIfUserIsOnlineQuery> validator,
        ILogger<CheckIfUserIsOnlineQueryHandler> logger,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<CheckIfUserIsOnlineQueryResult>> InternalHandleAsync(CheckIfUserIsOnlineQuery request, CancellationToken cancellationToken)
    {
        (bool cacheHit, string result) = await _hybridCacheProvider.GetOrDefaultAsync<string>(CacheKeyGenerator.UserIsLoggedInKey(request.UserId), cancellationToken);

        bool isOnline = cacheHit && !string.IsNullOrWhiteSpace(result);

        return HandlerResult.Create(new CheckIfUserIsOnlineQueryResult(isOnline));
    }
}