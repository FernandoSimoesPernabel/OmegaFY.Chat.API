using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Application.Events.Auth.Login;

internal sealed class MarkUserAsLoggedInEventHandler : EventHandlerHandlerBase<UserLoggedInEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public MarkUserAsLoggedInEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(UserLoggedInEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.SetUserIsLoggedInAsync(@event.UserId.ToString(), cancellationToken);
    }
}