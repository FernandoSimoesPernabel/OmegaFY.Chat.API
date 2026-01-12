using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Users.SendFriendshipRequest;

internal sealed class ExpireFriendshipCacheFromFriendshipRequestedEventHandler : EventHandlerHandlerBase<FriendshipRequestedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireFriendshipCacheFromFriendshipRequestedEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(FriendshipRequestedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.UsersUserIdTag(@event.RequestingUserId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.UsersUserIdTag(@event.InvitedUserId), cancellationToken);
    }
}