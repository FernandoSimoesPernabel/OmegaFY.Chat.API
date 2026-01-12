using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Application.Events.Auth.RefreshToken;

internal sealed class ExpireUsedRefreshTokenEventHandler : EventHandlerHandlerBase<UserTokenRefreshedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireUsedRefreshTokenEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected async override Task HandleAsync(UserTokenRefreshedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveAuthenticationTokenCacheAsync(@event.UserId, @event.OldRefreshToken, cancellationToken);
    }
}