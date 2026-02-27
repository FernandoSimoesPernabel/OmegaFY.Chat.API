using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Auth.Logoff;

internal sealed class MarkUserAsLoggedOffEventHandler : EventHandlerHandlerBase<UserLoggedOffEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public MarkUserAsLoggedOffEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected async override Task HandleAsync(UserLoggedOffEvent @event, CancellationToken cancellationToken) 
        => await _hybridCacheProvider.RemoveAsync(CacheKeyGenerator.UserIsLoggedInKey(@event.UserId), cancellationToken);
}