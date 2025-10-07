using Microsoft.Extensions.Caching.Hybrid;
using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Application.Events.Auth.RefreshToken;

internal sealed class ExpireUsedRefreshTokenEventHandler : EventHandlerHandlerBase<UserTokenRefreshedEvent>
{
    private readonly HybridCache _hybridCache;

    public ExpireUsedRefreshTokenEventHandler(HybridCache hybridCache) => _hybridCache = hybridCache;

    protected async override Task HandleAsync(UserTokenRefreshedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCache.RemoveAuthenticationTokenCacheAsync(@event.UserId, @event.OldRefreshToken, cancellationToken);
    }
}