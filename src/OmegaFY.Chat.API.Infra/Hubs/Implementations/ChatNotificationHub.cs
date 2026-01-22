using Microsoft.AspNetCore.SignalR;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;

namespace OmegaFY.Chat.API.Infra.Hubs.Implementations;

internal sealed class ChatNotificationHub : Hub<IChatNotificationHub>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ChatNotificationHub(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    public override async Task OnConnectedAsync()
    {
        await _hybridCacheProvider.SetAsync(
            CacheKeyGenerator.UserIsLoggedInKey(Context.UserIdentifier),
            Context.UserIdentifier,
            new CacheOptions(TimeSpan.FromHours(1)),
            CancellationToken.None);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await _hybridCacheProvider.RemoveAsync(CacheKeyGenerator.UserIsLoggedInKey(Context.UserIdentifier), CancellationToken.None);
        await base.OnDisconnectedAsync(exception);
    }

    //private async Task JoinGroupAsync(string groupName) => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

    //private async Task LeaveGroupAsync(string groupName) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
}