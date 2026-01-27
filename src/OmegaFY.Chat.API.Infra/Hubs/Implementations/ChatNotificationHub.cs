using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Infra.Hubs.Implementations;

[Authorize]
internal sealed class ChatNotificationHub : Hub<IChatNotificationHub>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ChatNotificationHub(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    public async override Task OnConnectedAsync()
    {
        await _hybridCacheProvider.SetUserIsLoggedInAsync(Context.UserIdentifier, Context.ConnectionAborted);
        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception exception) => await base.OnDisconnectedAsync(exception);

    //private async Task JoinGroupAsync(string groupName) => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

    //private async Task LeaveGroupAsync(string groupName) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
}