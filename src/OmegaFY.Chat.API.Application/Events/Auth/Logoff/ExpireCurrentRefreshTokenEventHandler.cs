using Microsoft.Extensions.Caching.Hybrid;
using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Application.Events.Auth.Logoff;

internal sealed class ExpireCurrentRefreshTokenEventHandler : EventHandlerHandlerBase<UserLoggedOffEvent>
{
    private readonly HybridCache _hybridCache;

    public ExpireCurrentRefreshTokenEventHandler(HybridCache hybridCache) => _hybridCache = hybridCache;

    protected async override Task HandleAsync(UserLoggedOffEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCache.RemoveAuthenticationTokenCacheAsync(@event.UserId, @event.CurrentRefreshToken, cancellationToken);
    }
}