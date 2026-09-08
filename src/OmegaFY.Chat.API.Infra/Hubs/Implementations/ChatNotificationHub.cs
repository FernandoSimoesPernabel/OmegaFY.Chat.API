using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Infra.Hubs.Implementations;

[Authorize(PoliciesNamesConstants.BEARER_JWT_POLICY)]
internal sealed class ChatNotificationHub : Hub<IChatNotificationHub>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ChatNotificationHub(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    public async override Task OnConnectedAsync()
    {
        await _hybridCacheProvider.SetUserIsLoggedInAsync(Context.UserIdentifier, Context.ConnectionAborted);
        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception exception)
    {
        await _hybridCacheProvider.RemoveAsync(CacheKeyGenerator.UserIsLoggedInKey(Context.UserIdentifier), Context.ConnectionAborted);
        await base.OnDisconnectedAsync(exception);
    }
}