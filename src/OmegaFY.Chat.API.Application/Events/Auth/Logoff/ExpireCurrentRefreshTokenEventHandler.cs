using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Application.Events.Auth.Logoff;

internal sealed class ExpireCurrentRefreshTokenEventHandler : EventHandlerHandlerBase<UserLoggedOffEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireCurrentRefreshTokenEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected async override Task HandleAsync(UserLoggedOffEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveAuthenticationTokenCacheAsync(@event.UserId, @event.CurrentRefreshToken, cancellationToken);
    }
}